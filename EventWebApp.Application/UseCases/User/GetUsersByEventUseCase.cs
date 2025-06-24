using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class GetUsersByEventUseCase
  {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public GetUsersByEventUseCase(IUserRepository userRepository, IMapper mapper)
    {
      this.userRepository = userRepository;
      this.mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> ExecuteAsync(Guid evId)
    {
      var users = await userRepository.GetUsersByEvent(evId);
      return mapper.Map<IEnumerable<UserDto>>(users);
    }
  }
}
