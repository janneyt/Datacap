using System.Collections.Generic;
using Datacap.Models;
using Datacap.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Datacap.Services
{
    public class TransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly FilePaths _filePaths;
        private readonly ILogger<TransactionsService> _logger;

        public TransactionsService(ITransactionsRepository transactionsRepository, IOptions<FilePaths> filePaths, ILogger<TransactionsService> logger)
        {
            _transactionsRepository = transactionsRepository;
            _filePaths = filePaths.Value;
            _logger = logger;
        }

        public List<TransactionDTO> GetTransactions()
        {
            _logger.LogInformation("Getting all transactions");
            // Here you can use _filePaths.Path1 and _filePaths.Path2
            return _transactionsRepository.GetTransactions();
        }
    }
}

