using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using AutoMapper;

namespace Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Collaboration, CollaborationDto>();
            CreateMap<CollaborationDto, Collaboration>();
            CreateMap<CollaborationUpdateDto, Collaboration>();
            CreateMap<CollaborationCreationDto, Collaboration>();
        }
    }
}