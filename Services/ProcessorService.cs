using Datacap.Models.DTO_Models;
using Datacap.Models;
using Datacap.Validators;
using FluentValidation;
using FluentValidation.Results;

public class ProcessorService
{
    private List<ProcessorDTO> Processors;

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

    public bool IsValidProcessorName(string processorName)
    {
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x)
            .NotEmpty().WithMessage("Processor name cannot be empty")
            .Length(1, 100).WithMessage("Processor name must be between 1 and 100 characters");

        ValidationResult validationResult = validator.Validate(processorName);
        return validationResult.IsValid;
    }


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

    public FeeRuleDTO GetFeeRuleForProcessor(string processorName)
    {
        var processor = Processors.FirstOrDefault(p => p.ProcessorName == processorName);
        return processor?.FeeRule;
    }


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




    public List<string> GetDefaultProcessors()
    {
        return Processors.Select(f => f.ProcessorName).ToList();
    }

    public bool IsValidTransactionAmount(decimal transactionAmount)
    {
        var validator = new InlineValidator<decimal>();
        validator.RuleFor(x => x).GreaterThanOrEqualTo(0).WithMessage("Transaction amount must be greater than or equal to 0");

        ValidationResult validationResult = validator.Validate(transactionAmount);
        return validationResult.IsValid;
    }

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

