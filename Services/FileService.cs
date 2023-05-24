using System.IO;
using System.Xml.Linq;
using System.Xml;
using Datacap.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Datacap.Services
{
    public class FileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public StreamReader OpenFile(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                _logger.LogError($"The file {filePath} does not exist.");
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            // Check if the file can be read
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            if (!fileStream.CanRead)
            {
                _logger.LogError($"The file {filePath} cannot be read.");
                throw new IOException($"The file {filePath} cannot be read.");
            }

            // Return a stream for reading
            return new StreamReader(fileStream);
        }

        /// <summary>
        /// Formats a TransactionDTO to save to a file (or other data storage)
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static string TransactionDTOToString(TransactionDTO transaction)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(transaction.TransactionID);
            stringBuilder.Append(",");
            stringBuilder.Append(transaction.Amount);
            stringBuilder.Append(",");
            stringBuilder.Append(transaction.ProcessorName);
            stringBuilder.Append(",");
            stringBuilder.Append(transaction.TransactionType.ToString());
            stringBuilder.Append(",");
            stringBuilder.Append(transaction.Fee);
            stringBuilder.Append(",");
            stringBuilder.Append(transaction.Rank);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Takes a string and converts to a TransactionDTO. For use with reading from files
        /// </summary>
        /// <param name="transactionString"></param>
        /// <returns></returns>
        public static TransactionDTO StringToTransactionDTO(string transactionString)
        {
            var parts = transactionString.Split(",");

            return new TransactionDTO
            {
                TransactionID = int.Parse(parts[0]),
                Amount = decimal.Parse(parts[1]),
                ProcessorName = parts[2],
                TransactionType = TransactionTypeDTO.Parse(parts[3]),
                Fee = decimal.Parse(parts[4]),
                Rank = int.Parse(parts[5])
            };
        }


        /// <summary>
        /// Saves a list of transactions to a file. Different from the XML because nodes and such have been stripped away, we're dealing with Transaction DTOs
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="filePath"></param>
        public void SaveTransactionsToFile(List<TransactionDTO> transactions, string filePath)
        {

            var transactionStrings = transactions.Select(TransactionDTOToString).ToList();
            File.WriteAllLines(filePath, transactionStrings);

        }

        /// <summary>
        /// XML reader that manually takes XML elements out of the file and converts to Transaction DTOs
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<XElement>> GetXmlElementsFromFileAsync(string filePath, string elementName)
        {
            var elements = new List<XElement>();

            using var stream = OpenFile(filePath);

            var settings = new XmlReaderSettings { Async = true };
            using var reader = XmlReader.Create(stream, settings);

            // Read and process the XML nodes
            while (await reader.ReadAsync())
            {
                // Only process elements with the specified name
                if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName)
                {
                    // Load the element directly into an XElement
                    var xml = XElement.Load(reader.ReadSubtree());

                    elements.Add(xml);
                }
            }

            return elements;
        }

        /// <summary>
        /// Opens a file and converts to DTO. Used mostly with the void transactions service
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<TransactionDTO> LoadTransactionsFromFile(string filePath)
        {

            var transactionStrings = File.ReadAllLines(filePath);
            var transactions = transactionStrings.Select(StringToTransactionDTO).ToList();

            return transactions;
        }


    }

}

