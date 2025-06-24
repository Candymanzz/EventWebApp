using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class CancelUserFromEventUseCase
  {
    private readonly IUserRepository userRepository;

    public CancelUserFromEventUseCase(IUserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    public async Task ExecuteAsync(Guid userId, Guid eventId)
    {
      var result = await userRepository.CancelUserFromEvent(userId, eventId);
      if (!result)
      {
        throw new NotFoundException("User or Event not found, or user is not registered for this event", ErrorCodes.NotFound);
      }
    }
  }
}
