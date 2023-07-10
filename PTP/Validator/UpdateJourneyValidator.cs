using FluentValidation;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Enums;
using PTP.Core.Dtos;

namespace PTP.Validator
{
    public class UpdateJourneyValidator : AbstractValidator<UpsertJourneyRequestDto>
    {
        public UpdateJourneyValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().GreaterThan(0).WithMessage("Journey Id is needed for update");
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Journey name is required.");
            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Journey description is required.");
            RuleFor(dto => dto.CurrencyId)
                .NotEmpty().WithMessage("Journey need to be associate with a currency");
            RuleFor(dto => dto.CountryId)
                .NotEmpty().WithMessage("Journey need to be associate with a country");
            RuleFor(dto => dto.PlaceId)
                .NotEmpty().WithMessage("Journey need to be associate with a location");
            RuleFor(dto => dto.Amount)
                .NotEmpty().GreaterThan(0).WithMessage("Journey need to be associate with an amount");
            RuleFor(dto => dto.CurrencyName)
                .NotEmpty().WithMessage("Currency is required.");
            RuleFor(dto => dto.CountryName)
                .NotEmpty().WithMessage("Country is required.");
            RuleFor(dto => dto.PlaceName)
                .NotEmpty().WithMessage("Places is required.");

            RuleFor(dto => dto.StartDate)
                .NotEmpty().WithMessage("Journey need to have a start date");

            RuleFor(dto => dto.EndDate)
                .NotEmpty().WithMessage("Journey need to have a end date");
            RuleFor(dto => dto.EndDate)
                .GreaterThan(dto => dto.StartDate).WithMessage("End date must be after start date");

            RuleFor(dto => dto.Status)
                .Must(value => Enum.IsDefined(typeof(JourneyStatus), value)).WithMessage("Invalid value for journey Status.");

            RuleFor(dto => dto.Version)
                .NotEmpty().WithMessage("Version is needed to update journey");
        }

    }
}
