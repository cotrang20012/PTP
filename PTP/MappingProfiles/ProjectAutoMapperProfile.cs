using AutoMapper;
using PTP.Core.Domain.Entities;
using PTP.Core.Dtos;

namespace PTP.MappingProfiles
{
    public class ProjectAutoMapperProfile : Profile
    {
        public ProjectAutoMapperProfile() 
        {
            CreateMap<Journey, JourneyDto>();
            CreateMap<UpsertJourneyRequestDto, Journey>();
            CreateMap<UpsertPlaceRequestDto, Place>();
            CreateMap<UpsertCountryRequestDto, Country>();
            CreateMap<UpsertCurrencyRequestDto, Currency>();
        }
    }
}
