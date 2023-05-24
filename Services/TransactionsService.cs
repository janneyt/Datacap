using Datacap.Models.DTO_Models;
using Datacap.Models;
using Datacap.Repositories;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using System.Xml;
using System.Transactions;

namespace Datacap.Services
{
    public class TransactionsService
    {
        private readonly ITransactionRespository _transactionsRepository;
        private readonly FilePaths _filePaths;
        private readonly ILogger<TransactionsService> _logger;
        private readonly ProcessorService _processorService;
        private readonly FileService _fileService;
        private readonly List<ProcessorDTO> _defaultProcessors;

        public TransactionsService(ITransactionRespository transactionsRepository, IOptions<FilePaths> filePaths, 
            ILogger<TransactionsService> logger, ProcessorService processorService, IConfiguration configuration,
            FileService fileService, List<ProcessorDTO> defaultProcessors)
        {
            // Dependency Injection means these are all created in Program.cs and set to private members here
            _processorService = processorService;

            // In-memory replacement for a database or file system
            _transactionsRepository = transactionsRepository;
            _filePaths = filePaths.Value;
            _filePaths.Initialize(configuration);
            _logger = logger;

            // More correctly, File I/O for the provided files.
            _fileService = fileService;

            _defaultProcessors = defaultProcessors;
        }


        private List<ProcessorDTO> GetProcessors()
        {
            var allTransactions = _transactionsRepository.GetAllTransactions();
            var groupedTransactions = allTransactions.GroupBy(t => t.ProcessorName);

            // Having gotten the transactions associated with this processor, it's time to arrange them, calculate the total fee
            // and prepare for return
            var processors = groupedTransactions.Select(group =>
            {
                var processor = new ProcessorDTO
                {
                    ProcessorName = group.Key,
                    Transactions = group.ToList(),
                    FeeRule = _processorService.GetFeeRuleForProcessor(group.Key) // retrieve FeeRule for this processor
                };

                _processorService.CalculateTotalFee(processor);
                return processor;
            }).ToList();

            return processors;
        }
        public async Task<List<ProcessorDTO>> GetAllProcessorsAsync(bool isItVoidTransaction)
        {
            // Ensure all transactions are processed first
            await ProcessTransactionsAsync(isItVoidTransaction);
            return GetProcessors();
        }


        public void AddTransaction(TransactionDTO transaction)
        {
            // The current fee rule is defined up in Program.cs, this just looks it up
            var processor = _processorService.GetProcessor(transaction.ProcessorName);
            if (processor == null)
            {
                throw new Exception($"Processor '{transaction.ProcessorName}' not found.");
            }

            // Access the FeeRule property of the processor
            var feeRule = processor.FeeRule;

            // My understanding is that the total fee is a combination of percentage and flat fees, so this calculates that as per the table
            // in the requirements
            var feeRate = _processorService.GetFeeRate(feeRule, transaction.Amount);
            var flatFee = _processorService.GetFlatFee(feeRule, transaction.Amount);
            transaction.Fee = transaction.Amount * feeRate.PercentageRate + flatFee;

            // And save it for later recall (such as the response)
            _transactionsRepository.AddTransaction(transaction);
        }

        // Modify AddTransactionFromXml to accept a processor
        private void AddTransactionFromXml(XElement transactionXml, ProcessorDTO processor)
        {
            // Parse the transaction type DTO from the XML
            var transactionType = new TransactionTypeDTO
            {
                TypeName = transactionXml.Element("type").Value,
                // Handle the TransactionTypeID accordingly, here assuming a default value
                TransactionTypeID = 1
            };

            // Create a new TransactionDTO from the XElement
            var transaction = new TransactionDTO
            {
                TransactionID = int.Parse(transactionXml.Element("refNo").Value),
                Amount = decimal.Parse(transactionXml.Element("amount").Value),
                TransactionType = transactionType,
                ProcessorName = processor.ProcessorName // Assign the ProcessorName to the Transaction
            };

            // Add the new transaction to the processor's transactions
            processor.Transactions.Add(transaction);
            _transactionsRepository.AddTransaction(transaction);
        }

        public async Task ProcessTransactionsAsync(bool isVoidTransaction)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
            var fileName = isVoidTransaction ? _filePaths.Transactions_Voids : _filePaths.Transactions_Sales;

            // Get all "tran" elements from the file
            var transactionsXml = await _fileService.GetXmlElementsFromFileAsync(fileName, "tran");
            _transactionsRepository.PrintRepository();
            // Process each transaction
            foreach (var xml in transactionsXml)
            {
                var transactionId = int.Parse(xml.Element("refNo").Value);

                if (isVoidTransaction)
                {
                    foreach (var processor in _defaultProcessors)
                    {
                        var transaction = _transactionsRepository.GetTransaction(transactionId);
                        Console.WriteLine($"Unsaved transaction {transaction}");

                        if (transaction != null)
                        {
                            processor.Transactions.Remove(transaction);
                            _processorService.CalculateTotalFee(processor);
                        }
                    }
                }
                else
                {
                    // Iterate over each default processor and add the transaction
                    foreach (var processor in _defaultProcessors)
                    {
                        AddTransactionFromXml(xml, processor);
                    }
                }
            }
            // Rank processors based on total fee after processing the transactions
            var processors = GetProcessors();
            var rankingService = new ProcessorRankingService();
            var rankings = rankingService.RankProcessors(processors);

            // Update the rank of each processor
            foreach (var processor in processors)
            {
                var processorRanking = rankings.FirstOrDefault(ranking => ranking.ProcessorName == processor.ProcessorName);
                if (processorRanking != null)
                {
                    processor.Rank = processorRanking.Rank;
                }
            }

            // Call scope.Complete() when the transaction is successful
            scope.Complete();
            }
        }

    }
}
