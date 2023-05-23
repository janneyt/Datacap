using System.IO;
using Microsoft.Extensions.Configuration;

namespace Datacap.Models
{
    public class FilePaths
    {
        public string Transactions_Sales { get; }
        public string Transactions_Voids { get; }

        public FilePaths(IConfiguration configuration)
        {
            var resourcesPath = Path.Combine(configuration.GetValue<string>("AppRootPath"), "Resources");

            Transactions_Sales = Path.Combine(resourcesPath, "Transactions_Sales.txt");
            Transactions_Voids = Path.Combine(resourcesPath, "Transactions_Voids.txt");

            // Check if the directory exists
            if (!Directory.Exists(resourcesPath))
            {
                throw new DirectoryNotFoundException($"The directory {resourcesPath} does not exist");
            }

            // Check if the files exist
            if (!File.Exists(Transactions_Sales))
            {
                throw new FileNotFoundException($"The file {Transactions_Sales} does not exist");
            }

            if (!File.Exists(Transactions_Voids))
            {
                throw new FileNotFoundException($"The file {Transactions_Voids} does not exist");
            }
        }
    }
}


