using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class CancelUserFromEventUseCase
  {
    private readonly IUserRepository userRepository;
    private readonly IEventRepository eventRepository;

    public CancelUserFromEventUseCase(
        IUserRepository userRepository,
        IEventRepository eventRepository)
    {
      this.userRepository = userRepository;
      this.eventRepository = eventRepository;
    }

    public async Task ExecuteAsync(Guid userId, Guid eventId)
    {
      var user = await userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        throw new NotFoundException("User not found", ErrorCodes.NotFound);
      }

      var _event = await eventRepository.GetByIdAsync(eventId);
      if (_event == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      if (!_event.Users.Contains(user))
      {
        throw new NotFoundException("User is not registered for this event", ErrorCodes.NotFound);
      }

      _event.Users.Remove(user);
      await eventRepository.UpdateAsync(_event);
    }
  }
}
