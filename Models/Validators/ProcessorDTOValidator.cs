using Datacap.Models;
using FluentValidation;

namespace Datacap.Validators
{
    public class ProcessorDTOValidator : AbstractValidator<ProcessorDTO>
    {
        public ProcessorDTOValidator()
        {
            RuleFor(x => x.ProcessorName)
                .NotEmpty().WithMessage("Please enter a processor name")
                .Length(1, 100).WithMessage("Processor name must be between 1 and 100 characters");

            RuleFor(x => x.Transactions)
                .NotNull().WithMessage("Transactions list cannot be null");

            RuleFor(x => x.TotalFee)
                .GreaterThanOrEqualTo(0).WithMessage("Total fee must be greater than or equal to 0");

            RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0).WithMessage("Rank must be greater than or equal to 0");

            RuleFor(x => x.FeeRule)
                .NotNull().WithMessage("Fee rule cannot be null");
        }
    }
}
