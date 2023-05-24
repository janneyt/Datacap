using FluentValidation;
using Datacap.Models;

public class TransactionDTOValidator : AbstractValidator<TransactionDTO>
{
    public TransactionDTOValidator()
    {
        RuleFor(x => x.TransactionID).GreaterThan(0).WithMessage("Transaction ID must be greater than 0");
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Transaction amount must be greater than or equal to 0");
        RuleFor(x => x.ProcessorName).NotEmpty().WithMessage("Processor name cannot be empty");
        RuleFor(x => x.TransactionType).NotNull().WithMessage("Transaction type cannot be null");
        RuleFor(x => x.Fee).GreaterThanOrEqualTo(0).WithMessage("Fee must be greater than or equal to 0");
        RuleFor(x => x.Rank).GreaterThanOrEqualTo(0).WithMessage("Rank must be greater than or equal to 0");
    }
}

