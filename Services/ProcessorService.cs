using Datacap.Models.DTO_Models;
using Datacap.Models;
using Datacap.Validators;
using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

public class ProcessorService
{
    private List<ProcessorDTO> Processors;

    /// <summary>
    /// The processor is a payment processor. The processor service mostly helps store and retrieve the differences in fee processing
    /// </summary>
    /// <param name="defaultProcessors"></param>
    /// <exception cref="ValidationException"></exception>
    public ProcessorService(List<ProcessorDTO> defaultProcessors)
    {
        var validator = new ProcessorDTOValidator();
        Processors = new List<ProcessorDTO>();

        foreach (var processor in defaultProcessors)
        {
            ValidationResult validationResult = validator.Validate(processor);
            if (validationResult.IsValid)
            {
                Processors.Add(processor);
            }
            else
            {
                throw new ValidationException("Validation exception in constructor for processor");
            }
        }
    }

    /// <summary>
    /// This would be used to extend and modify behavior by adding to the default list of processors in Program.cs
    /// </summary>
    /// <param name="newProcessor"></param>
    /// <exception cref="ValidationException"></exception>
    public void AddProcessor(ProcessorDTO newProcessor)
    {
        var validator = new ProcessorDTOValidator();
        ValidationResult validationResult = validator.Validate(newProcessor);

        if (validationResult.IsValid)
        {
            Processors.Add(newProcessor);
        }
        else
        {
            throw new ValidationException("Validation exception in add processor");
        }
    }

    /// <summary>
    /// Primarily a validation function, it makes sure the string is valid
    /// </summary>
    /// <param name="processorName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool IsValidProcessorName(string processorName)
    {
        if (processorName == null)
        {
            throw new ArgumentNullException(nameof(processorName), "Processor name cannot be null");
        }

        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x)
            .NotEmpty().WithMessage("Processor name cannot be empty")
            .Length(1, 100).WithMessage("Processor name must be between 1 and 100 characters");

        ValidationResult validationResult = validator.Validate(processorName);
        return validationResult.IsValid;
    }

    /// <summary>
    /// Getter method with validation. Returns a specific processor or throws an exception if processor doesn't exist
    /// </summary>
    /// <param name="processorName"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public ProcessorDTO GetProcessor(string processorName)
    {
        if (IsValidProcessorName(processorName))
        {
            return Processors.FirstOrDefault(r => r.ProcessorName == processorName);
        }
        else
        {
            throw new ValidationException("Cannot validate processor name");
        }
    }

    /// <summary>
    /// FeeRules are bound to the processor, so this is an access method for the fee rule on a specific processor
    /// </summary>
    /// <param name="processorName"></param>
    /// <returns></returns>
    public FeeRuleDTO GetFeeRuleForProcessor(string processorName)
    {
        if (IsValidProcessorName(processorName))
        {
            var processor = Processors.FirstOrDefault(p => p.ProcessorName == processorName);
            return processor?.FeeRule;
        }
        else
        {
            throw new ValidationException("Processor name is invalied");
        }
    }

    /// <summary>
    /// This calculates the total fee per processor. Total fee is calculated via the formula FlatFee + Amount * Percentage Fee
    /// </summary>
    /// <param name="processor"></param>
    /// <exception cref="ValidationException"></exception>
    public void CalculateTotalFee(ProcessorDTO processor)
    {
        var validator = new ProcessorDTOValidator();
        ValidationResult validationResult = validator.Validate(processor);

        if (validationResult.IsValid)
        {
            processor.TotalFee = processor.Transactions.Sum(t =>
                GetFlatFee(processor.FeeRule, t.Amount) +
                t.Amount * GetFeeRate(processor.FeeRule, t.Amount).PercentageRate);
        }
        else
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
            throw new ValidationException($"Cannot validate processor for calculating the total fee: {string.Join(", ", errorMessages)} ProcessorName: {processor.ProcessorName}");
        }
    }
    
    /// <summary>
    /// Access method for my default processors in Program.cs.
    /// </summary>
    /// <returns></returns>
    public List<string> GetDefaultProcessors()
    {
        return Processors.Select(f => f.ProcessorName).ToList();
    }

    /// <summary>
    /// Inline validator for transaction amounts as that doesn't have a DTO
    /// </summary>
    /// <param name="transactionAmount"></param>
    /// <returns></returns>
    public bool IsValidTransactionAmount(decimal transactionAmount)
    {
        var validator = new InlineValidator<decimal>();
        validator.RuleFor(x => x).GreaterThanOrEqualTo(0).WithMessage("Transaction amount must be greater than or equal to 0");

        ValidationResult validationResult = validator.Validate(transactionAmount);
        return validationResult.IsValid;
    }

    /// <summary>
    /// Access method for the fee rate, which is stored as a DTO in the Processor DTO, hence the access method nesting.
    /// </summary>
    /// <param name="feeRule"></param>
    /// <param name="transactionAmount"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public RateDTO GetFeeRate(FeeRuleDTO feeRule, decimal transactionAmount)
    {
        var feeRuleValidator = new FeeRuleDTOValidator();
        ValidationResult feeRuleValidationResult = feeRuleValidator.Validate(feeRule);

        if (feeRuleValidationResult.IsValid && IsValidTransactionAmount(transactionAmount))
        {
            return transactionAmount < 50 ? feeRule.SmallTransactionRate : feeRule.LargeTransactionRate;
        }
        else
        {
            throw new ValidationException("Cannot validate Fee Rate");
        }
    }

    /// <summary>
    /// This actually works on the business logic from the example. The is the flat fee, as opposed to the percentage fee.
    /// </summary>
    /// <param name="feeRule"></param>
    /// <param name="transactionAmount"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public decimal GetFlatFee(FeeRuleDTO feeRule, decimal transactionAmount)
    {
        var feeRuleValidator = new FeeRuleDTOValidator();
        ValidationResult feeRuleValidationResult = feeRuleValidator.Validate(feeRule);

        if (feeRuleValidationResult.IsValid && IsValidTransactionAmount(transactionAmount))
        {
            return transactionAmount < 50 ? feeRule.SmallTransactionFlatFee : feeRule.LargeTransactionFlatFee;
        }
        else
        {
            throw new Exception("Cannot validate flat fee");
        }
    }

}

