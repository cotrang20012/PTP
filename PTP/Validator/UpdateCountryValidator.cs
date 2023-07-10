using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class UpdateCountryValidator : AbstractValidator<UpsertCountryRequestDto>
    {
        public UpdateCountryValidator() 
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().GreaterThan(0).WithMessage("Country Id is needed for update");
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Country name is required.");
            RuleFor(dto => dto.Version)
                    .NotEmpty().WithMessage("Version is needed to update country");
        }
    }
}
