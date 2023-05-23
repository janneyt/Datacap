using System.Collections.Generic;
using Datacap.Models;
using Datacap.Repositories;
namespace Datacap.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRespository
    {
        private List<TransactionDTO> transactions = new List<TransactionDTO>();

        public List<TransactionDTO> GetTransactions()
        {
            return transactions;
        }

        public void AddTransaction(TransactionDTO transaction)
        {
            transactions.Add(transaction);
        }

        // Implement the GetAllTransactions method
        public List<TransactionDTO> GetAllTransactions()
        {
            return transactions;
        }
    }
}
