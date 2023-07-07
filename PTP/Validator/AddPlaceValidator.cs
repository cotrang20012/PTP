using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class AddPlaceValidator : AbstractValidator<UpsertPlaceRequestDto>
    {
        public AddPlaceValidator() 
        { 
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Place need to have a name");
            RuleFor(dto => dto.CountryId)
                .NotEmpty().GreaterThan(0).WithMessage("Place need to be associate with a Country");
        }
    }
}
