using Datacap.Models;
using Datacap.Models.DTO_Models;

public class ProcessorService
{
    private List<FeeRuleDTO> FeeRules;

    public ProcessorService(List<FeeRuleDTO> defaultFeeRules)
    {
        FeeRules = defaultFeeRules;
    }

    public void AddProcessor(FeeRuleDTO newFeeRule)
    {
        FeeRules.Add(newFeeRule);
    }

    public FeeRuleDTO GetProcessor(string processorName)
    {
        return FeeRules.FirstOrDefault(r => r.ProcessorName == processorName);
    }

    public RateDTO GetFeeRate(FeeRuleDTO feeRule, decimal transactionAmount)
    {
        return transactionAmount < 50 ? feeRule.SmallTransactionRate : feeRule.LargeTransactionRate;
    }

    public decimal GetFlatFee(FeeRuleDTO feeRule, decimal transactionAmount)
    {
        return transactionAmount < 50 ? feeRule.SmallTransactionFlatFee : feeRule.LargeTransactionFlatFee;
    }
    public void CalculateTotalFee(ProcessorDTO processor)
    {
        processor.TotalFee = processor.Transactions.Sum(t => t.Fee);
    }
    public List<string> GetDefaultProcessors()
    {
        return FeeRules.Select(f => f.ProcessorName).ToList();
    }
}