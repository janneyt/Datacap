using System.Collections.Generic;
using System.Linq;
using Datacap.Models;
using Datacap.Models.DTO_Models;

namespace Datacap.Services
{
    public class ProcessorRankingService
    {
        public List<RankingDTO> RankProcessors(List<ProcessorDTO> processors)
        {
            return processors
                .Select(processor => new ProcessorTotalDTO
                {
                    ProcessorName = processor.ProcessorName,
                    TotalAmount = processor.Transactions.Sum(transaction => transaction.Amount)
                })
                .OrderByDescending(processor => processor.TotalAmount)
                .Select((processor, index) => new RankingDTO
                {
                    ProcessorName = processor.ProcessorName,
                    Rank = index + 1
                })
                .ToList();
        }
    }
}

