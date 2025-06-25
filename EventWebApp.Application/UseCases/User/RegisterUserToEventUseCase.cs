using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserToEventUseCase
  {
    private readonly IUserRepository userRepository;
    private readonly IEventRepository eventRepository;

    public RegisterUserToEventUseCase(
        IUserRepository userRepository,
        IEventRepository eventRepository)
    {
      this.userRepository = userRepository;
      this.eventRepository = eventRepository;
    }

    public async Task ExecuteAsync(Guid userId, Guid eventId)
    {
      // Получение и проверка существования пользователя
      var user = await userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        throw new NotFoundException("User not found", ErrorCodes.NotFound);
      }

      // Получение и проверка существования события
      var _event = await eventRepository.GetByIdAsync(eventId);
      if (_event == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      // Проверка, не зарегистрирован ли пользователь уже на событие
      if (_event.Users.Contains(user))
      {
        throw new AlreadyExistsException(
            "User is already registered for this event.",
            ErrorCodes.UserAlreadyRegisteredForEvent
        );
      }

      // Проверка, не заполнено ли событие
      if (_event.Users.Count >= _event.MaxParticipants)
      {
        throw new ConflictException(
            "Event has reached maximum number of participants.",
            ErrorCodes.EventFull
        );
      }

      // Добавление пользователя к событию
      _event.Users.Add(user);
      await eventRepository.UpdateAsync(_event);
    }
  }
}
