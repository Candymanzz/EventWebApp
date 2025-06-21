using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(
                    dest => dest.CurrentParticipantsCount,
                    opt => opt.MapFrom(src => src.Users.Count)
                );

            CreateMap<CreateEventRequest, Event>();

            CreateMap<UpdateEventRequest, Event>();

            CreateMap<UserRegistrationRequest, User>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<User, UserDto>();
        }
    }
}
