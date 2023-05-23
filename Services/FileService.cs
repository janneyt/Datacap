using System.IO;
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
    }
}

