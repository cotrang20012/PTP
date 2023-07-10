using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class InsertNewCountryValidator : AbstractValidator<UpsertCountryRequestDto>
    {
        public InsertNewCountryValidator() 
        { 
            RuleFor(dto => dto.Name).NotEmpty().WithMessage("Country name is required.");
        }
    }
}
