using Datacap.Models.DTO_Models;
using FluentValidation;

namespace Datacap.Validators
{
    public class FeeRuleDTOValidator : AbstractValidator<FeeRuleDTO>
    {
        public FeeRuleDTOValidator()
        {
            RuleFor(x => x.ProcessorName)
                .NotEmpty().WithMessage("Please enter a processor name")
                .Length(1, 100).WithMessage("Processor name must be between 1 and 100 characters");

            RuleFor(x => x.SmallTransactionRate)
                .NotNull().WithMessage("Small transaction rate cannot be null");

            RuleFor(x => x.SmallTransactionFlatFee)
                .GreaterThanOrEqualTo(0).WithMessage("Small transaction flat fee must be greater than or equal to 0");

            RuleFor(x => x.LargeTransactionRate)
                .NotNull().WithMessage("Large transaction rate cannot be null");

            RuleFor(x => x.LargeTransactionFlatFee)
                .GreaterThanOrEqualTo(0).WithMessage("Large transaction flat fee must be greater than or equal to 0");
        }
    }
}

