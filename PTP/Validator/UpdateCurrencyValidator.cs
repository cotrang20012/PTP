using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class UpdateCurrencyValidator : AbstractValidator<UpsertCurrencyRequestDto>
    {
        public UpdateCurrencyValidator() 
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().GreaterThan(0).WithMessage("Currency Id is needed for update");
            RuleFor(dto => dto.Name).NotEmpty().WithMessage("Currency name is required.");
            RuleFor(dto => dto.Version)
                .NotEmpty().WithMessage("Version is needed to update currency");
        }
    }
}
