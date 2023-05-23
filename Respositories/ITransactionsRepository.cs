using System.Collections.Generic;
using Datacap.Models;

namespace Datacap.Repositories
{
    public interface ITransactionsRepository
    {
        List<TransactionDTO> GetTransactions();
    }
}

