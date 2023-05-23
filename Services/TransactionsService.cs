using Datacap.Models.DTO_Models;
using Datacap.Models;
using Datacap.Repositories;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using System.Xml;

namespace Datacap.Services
{
    public class TransactionsService
    {
        private readonly ITransactionRespository _transactionsRepository;
        private readonly FilePaths _filePaths;
        private readonly ILogger<TransactionsService> _logger;
        private readonly ProcessorService _processorService;
        private readonly FileService _fileService;

        public TransactionsService(ITransactionRespository transactionsRepository, IOptions<FilePaths> filePaths, ILogger<TransactionsService> logger, ProcessorService processorService, IConfiguration configuration, FileService fileService)
        {
            _processorService = processorService;
            _transactionsRepository = transactionsRepository;
            _filePaths = filePaths.Value;
            _filePaths.Initialize(configuration);
            _logger = logger;
            _fileService = fileService;
        }


        private List<ProcessorDTO> GetProcessors()
        {
            var allTransactions = _transactionsRepository.GetAllTransactions();
            var groupedTransactions = allTransactions.GroupBy(t => t.ProcessorName);
            var processors = groupedTransactions.Select(group =>
            {
                var processor = new ProcessorDTO
                {
                    ProcessorName = group.Key,
                    Transactions = group.ToList()
                };
                _processorService.CalculateTotalFee(processor);
                return processor;
            }).ToList();
            return processors;
        }
        public async Task<List<ProcessorDTO>> GetAllProcessorsAsync()
        {
            // Ensure all transactions are processed first
            await AddAllTransactionsAsync();
            return GetProcessors();
        }


        public void AddTransaction(TransactionDTO transaction)
        {
            var feeRule = _processorService.GetProcessor(transaction.ProcessorName);
            if (feeRule == null)
            {
                throw new Exception($"Processor '{transaction.ProcessorName}' not found.");
            }

            // Calculate fee
            var feeRate = _processorService.GetFeeRate(feeRule, transaction.Amount);
            var flatFee = _processorService.GetFlatFee(feeRule, transaction.Amount);
            transaction.Fee = transaction.Amount * feeRate.PercentageRate + flatFee;

            _transactionsRepository.AddTransaction(transaction);
        }
        public async Task<List<TransactionDTO>> AddAllTransactionsAsync()
        {
            var transactions = new List<TransactionDTO>();

            // Open the sales file
            using var stream = _fileService.OpenFile(_filePaths.Transactions_Sales);

            var settings = new XmlReaderSettings { Async = true };
            using var reader = XmlReader.Create(stream, settings);

            // Get all processors
            var processors = _processorService.GetDefaultProcessors();

            // Read and process the XML nodes
            while (await reader.ReadAsync())
            {
                // Only process "tran" elements
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "tran")
                {
                    // Load the "tran" element directly into an XElement
                    var xml = XElement.Load(reader.ReadSubtree());

                    foreach (var processorName in processors)
                    {
                        // Create a transaction for each processor from the XML data
                        var transaction = new TransactionDTO
                        {
                            TransactionID = int.Parse(xml.Element("refNo").Value),
                            Amount = decimal.Parse(xml.Element("amount").Value),
                            TransactionType = new TransactionTypeDTO
                            {
                                TypeName = xml.Element("type").Value
                            },
                            ProcessorName = processorName, // use the name of the processor in the loop
                            Rank = 0,  // Assign a default rank of 0
                            Fee = 1,
                        };

                        // Add the transaction
                        AddTransaction(transaction);
                        transactions.Add(transaction);
                    }
                }
            }

            // Calculate rank after all transactions have been added
            var processorDtos = GetProcessors();
            var rankingService = new ProcessorRankingService();
            var rankings = rankingService.RankProcessors(processorDtos);

            // Update the rank of each transaction
            foreach (var transaction in transactions)
            {
                var processorRanking = rankings.FirstOrDefault(ranking => ranking.ProcessorName == transaction.ProcessorName);
                if (processorRanking != null)
                {
                    transaction.Rank = processorRanking.Rank;
                }
            }

            return transactions;
        }



    }
}
