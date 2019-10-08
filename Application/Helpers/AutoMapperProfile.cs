using Application.Models.DataTransferObjects;
using Application.Models.Entities;

namespace Application.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, EventDto>();
            CreateMap<EventDto, Event>();
            CreateMap<EventCreationDto, Event>();
        }
    }
}