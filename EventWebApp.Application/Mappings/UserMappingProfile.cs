using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.Mappings
{
  public class UserMappingProfile : Profile
  {
    public UserMappingProfile()
    {
      CreateMap<UserRegistrationRequest, User>()
          .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

      CreateMap<User, UserDto>();
    }
  }
}