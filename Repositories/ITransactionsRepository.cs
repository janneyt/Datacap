using System.Collections.Generic;
using Datacap.Models;
using Datacap.Models.DTO_Models;

namespace Datacap.Repositories
{
    public interface ITransactionsRepository
    {
        List<TransactionResponse> GetTransactions();
    }
}

