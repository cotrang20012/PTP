using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class InsertNewCurrencyValidator : AbstractValidator<UpsertCurrencyRequestDto>
    {
        public InsertNewCurrencyValidator() 
        {
            RuleFor(dto => dto.Name).NotEmpty().WithMessage("Currency name is required.");
        }
    }
}
