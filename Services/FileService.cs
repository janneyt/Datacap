using System.IO;
using System.Xml.Linq;
using System.Xml;
using Datacap.Models;
using Microsoft.Extensions.Logging;

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

    }

}

