using FluentValidation;
using PTP.Core.Domain.Enums;
using PTP.Dtos;
using System;

namespace PTP.Validator
{
    public class AddJourneyValidator : AbstractValidator<UpsertJourneyRequest>
    {
        public AddJourneyValidator() 
        {
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

            RuleFor(dto => dto.StartDate)
                .NotEmpty().WithMessage("Journey need to have a start date");

            RuleFor(dto => dto.EndDate)
                .NotEmpty().WithMessage("Journey need to have a end date");
            RuleFor(dto => dto.EndDate)
                .GreaterThan(dto => dto.StartDate).WithMessage("End date must be after start date");

            RuleFor(dto => dto.Days)
                .NotEmpty().GreaterThan(0).GreaterThan(dto => dto.Nights).WithMessage("Journey need to have a duration, duration should be larger than the number of nights the journey will take");
            RuleFor(dto => dto.Nights)
                .NotEmpty().GreaterThan(0).WithMessage("Journey need to have a number that represent how many nights the journey will take");

            RuleFor(dto => dto.Status)
                .Must(value => Enum.IsDefined(typeof(JourneyStatus), value)).WithMessage("Invalid value for journey Status.");
        }
    }
}
