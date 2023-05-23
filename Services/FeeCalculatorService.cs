using Datacap.Models.DTO_Models;
using Datacap.Models;

public class FeeCalculator
{
    private List<FeeRuleDTO> FeeRules;

    public FeeCalculator(List<FeeRuleDTO> feeRules)
    {
        this.FeeRules = feeRules;
    }

    public decimal CalculateFee(TransactionDTO transaction)
    {
        var rule = FeeRules.First(r => r.ProcessorName == transaction.ProcessorName);

        var rate = transaction.Amount < 50 ? rule.SmallTransactionRate : rule.LargeTransactionRate;
        var flatFee = transaction.Amount < 50 ? rule.SmallTransactionFlatFee : rule.LargeTransactionFlatFee;

        return transaction.Amount * rate.PercentageRate + flatFee;
    }
}