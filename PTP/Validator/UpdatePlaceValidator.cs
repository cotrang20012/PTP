using FluentValidation;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class UpdatePlaceValidator : AbstractValidator<UpsertPlaceRequestDto>
    {
        public UpdatePlaceValidator() 
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().GreaterThan(0).WithMessage("Place Id is needed for update");
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Place name is required.");
            RuleFor(dto => dto.CountryId)
                .NotEmpty().GreaterThan(0).WithMessage("Place need to be associate with a Country");
            RuleFor(dto => dto.Version)
                .NotEmpty().WithMessage("Version is needed to update place");
        }
        
    }
}
