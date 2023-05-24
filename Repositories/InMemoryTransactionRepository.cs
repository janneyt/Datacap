using System.Collections.Generic;
using System.Transactions;
using Datacap.Models;
using Datacap.Repositories;
using System.Linq;
using Datacap.Models.DTO_Models; 

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
        public TransactionDTO GetTransaction(int transactionID)
        {
            return transactions.FirstOrDefault(t => t.TransactionID == transactionID);
        }

    }
}
