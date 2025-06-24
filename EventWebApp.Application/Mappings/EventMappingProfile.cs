using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.Mappings
{
  public class EventMappingProfile : Profile
  {
    public EventMappingProfile()
    {
      CreateMap<Event, EventDto>()
          .ForMember(
              dest => dest.CurrentParticipantsCount,
              opt => opt.MapFrom(src => src.Users.Count)
          );

      CreateMap<CreateEventRequest, Event>();

      CreateMap<UpdateEventRequest, Event>();
    }
  }
}