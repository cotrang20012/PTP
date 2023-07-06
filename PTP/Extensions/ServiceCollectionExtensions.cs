using FluentValidation;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;
using PTP.Dtos;
using PTP.Repositories;
using PTP.Services;
using PTP.Validator;

namespace PTP.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IJourneyService, JourneyService>();
            //services.AddScoped<AbstractValidator<UpsertJourneyRequest>, UpdateJourneyValidator>();
            //services.AddScoped<AbstractValidator<UpsertJourneyRequest>, AddJourneyValidator>();

        }
    }
}
