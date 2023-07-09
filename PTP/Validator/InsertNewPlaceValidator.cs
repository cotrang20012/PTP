using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class InsertNewPlaceValidator : AbstractValidator<UpsertPlaceRequestDto>
    {
        public InsertNewPlaceValidator() 
        { 
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Place need to have a name");
            RuleFor(dto => dto.CountryId)
                .NotEmpty().GreaterThan(0).WithMessage("Place need to be associate with a Country");
        }
    }
}
