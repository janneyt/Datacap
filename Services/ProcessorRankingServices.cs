using Datacap.Models.DTO_Models;
using Datacap.Models;
using System.Linq;

public class ProcessorRankingService
{
    public List<RankingDTO> RankProcessors(List<ProcessorDTO> processors)
    {
        // Compute TotalFee for each processor and sort
        var sortedProcessors = processors
            .Select(processor =>
            {
                processor.TotalFee = processor.Transactions.Sum(transaction => transaction.Fee);
                return processor;
            })
            .OrderByDescending(processor => processor.TotalFee)
            .ToList();

        // Compute the rankings
        var rankings = sortedProcessors
            .Select((processor, index) => new RankingDTO
            {
                ProcessorName = processor.ProcessorName,
                Rank = index + 1
            })
            .ToList();

        return rankings;
    }
}


