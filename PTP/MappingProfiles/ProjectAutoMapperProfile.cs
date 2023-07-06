using AutoMapper;
using PTP.Core.Domain.Entities;
using PTP.Dtos;

namespace PTP.MappingProfiles
{
    public class ProjectAutoMapperProfile : Profile
    {
        public ProjectAutoMapperProfile() 
        {
            CreateMap<Journey, JourneyDto>();
            CreateMap<UpsertJourneyRequest, Journey>();
        }
    }
}
