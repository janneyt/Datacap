using Datacap.Models.DTO_Models;
using Datacap.Models;
using FluentValidation;
using FluentValidation.Results;
using Datacap.Validators;

public class FeeCalculator
{
    private List<FeeRuleDTO> FeeRules;

    public FeeCalculator(List<FeeRuleDTO> feeRules)
    {
        this.FeeRules = feeRules;
    }

    private bool IsValidTransactionAndFeeRules(TransactionDTO transaction, out ValidationResult transactionValidationResult, out List<ValidationResult> feeRuleValidationResults)
    {
        var transactionValidator = new TransactionDTOValidator();
        transactionValidationResult = transactionValidator.Validate(transaction);

        var feeRuleValidator = new FeeRuleDTOValidator();
        feeRuleValidationResults = FeeRules.Select(feeRule => feeRuleValidator.Validate(feeRule)).ToList();

        return transactionValidationResult.IsValid && feeRuleValidationResults.All(result => result.IsValid);
    }

    public decimal CalculateFee(TransactionDTO transaction)
    {
        ValidationResult transactionValidationResult;
        List<ValidationResult> feeRuleValidationResults;
        if (IsValidTransactionAndFeeRules(transaction, out transactionValidationResult, out feeRuleValidationResults))
        {
            var rule = FeeRules.First(r => r.ProcessorName == transaction.ProcessorName);

            var rate = transaction.Amount < 50 ? rule.SmallTransactionRate : rule.LargeTransactionRate;
            var flatFee = transaction.Amount < 50 ? rule.SmallTransactionFlatFee : rule.LargeTransactionFlatFee;

            return transaction.Amount * rate.PercentageRate + flatFee;
        }
        else
        {
            throw new ValidationException("Could not validate the Transaction while calculating the fee");
        }
    }
}
