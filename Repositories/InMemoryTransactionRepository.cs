using System.Collections.Generic;
using System.Transactions;
using Datacap.Models;
using Datacap.Repositories;
using System.Linq;
using Datacap.Models.DTO_Models;
using Microsoft.Extensions.Logging;
using System;

namespace Datacap.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRespository
    {
        private List<TransactionDTO> transactions = new List<TransactionDTO>();
        private readonly ILogger<InMemoryTransactionRepository> _logger;

        public InMemoryTransactionRepository(ILogger<InMemoryTransactionRepository> logger)
        {
            _logger = logger;
        }

        // Mostly for debugging but if you ever want to see the repository :)
        public void PrintRepository()
        {
            _logger.LogInformation("Printing all transactions from repository");
            transactions.ForEach(Console.WriteLine);
        }


        public List<TransactionDTO> GetTransactions()
        {
            _logger.LogInformation("Getting all transactions from repository");
            return transactions;
        }

        public void AddTransaction(TransactionDTO transaction)
        {
            _logger.LogInformation($"Adding transaction with ID {transaction.TransactionID} to repository");
            transactions.Add(transaction);
            _logger.LogInformation($"Added transaction with ID {transaction.TransactionID} to repository");
        }

        // Implement the GetAllTransactions method
        public List<TransactionDTO> GetAllTransactions()
        {
            _logger.LogInformation("Getting all transactions from repository");
            return transactions;
        }
        public TransactionDTO GetTransaction(int transactionID)
        {
            _logger.LogInformation($"Getting transaction with ID {transactionID} from repository");
            var transaction = transactions.FirstOrDefault(t => t.TransactionID == transactionID);
            if (transaction == null)
            {
                _logger.LogWarning($"No transaction with ID {transactionID} found in repository");
            }
            return transaction;
        }
    }
}

