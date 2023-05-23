using Datacap.Models;
using System.Collections.Generic;

namespace Datacap.Repositories
{
    public interface ITransactionRespository
    {
        List<TransactionDTO> GetTransactions();
        void AddTransaction(TransactionDTO transaction);
        List<TransactionDTO> GetAllTransactions(); // Add this method
    }
}
