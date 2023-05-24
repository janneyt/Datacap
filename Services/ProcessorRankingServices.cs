using Datacap.Models.DTO_Models;
using Datacap.Models;
using System.Linq;
using Datacap.Validators;
using FluentValidation.Results;

public class ProcessorRankingService
{

    /// <summary>
    /// Helper and validator method for lists of Processors
    /// </summary>
    /// <param name="processors"></param>
    /// <param name="processorValidationResults"></param>
    /// <returns></returns>
    private bool AreProcessorsValid(List<ProcessorDTO> processors, out List<ValidationResult> processorValidationResults)
    {
        var processorValidator = new ProcessorDTOValidator();
        processorValidationResults = processors.Select(processor => processorValidator.Validate(processor)).ToList();

        return processorValidationResults.All(result => result.IsValid);
    }

    /// <summary>
    /// Calculates the Rank per processor
    /// </summary>
    /// <param name="processors"></param>
    /// <returns></returns>
    public List<RankingDTO> RankProcessors(List<ProcessorDTO> processors)
    {
        List<ValidationResult> processorValidationResults;
        if (AreProcessorsValid(processors, out processorValidationResults))
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
        else
        {
            // Handle validation errors
            // You can access validation errors using processorValidationResults
            return new List<RankingDTO>(); // Or return a default list of RankingDTO objects
        }
    }
}



