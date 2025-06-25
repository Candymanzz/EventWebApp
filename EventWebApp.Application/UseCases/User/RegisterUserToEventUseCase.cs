using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserToEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserToEventUseCase(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid userId, Guid eventId)
    {
      var user = await _unitOfWork.Users.GetByIdAsync(userId);
      if (user == null)
      {
        throw new NotFoundException("User not found", ErrorCodes.NotFound);
      }

      var _event = await _unitOfWork.Events.GetByIdAsync(eventId);
      if (_event == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      var isUserRegistered = await _unitOfWork.Events.IsUserRegisteredForEventAsync(userId, eventId);
      if (isUserRegistered)
      {
        throw new AlreadyExistsException(
            "User is already registered for this event.",
            ErrorCodes.UserAlreadyRegisteredForEvent
        );
      }

      if (_event.Users.Count >= _event.MaxParticipants)
      {
        throw new ConflictException(
            "Event has reached maximum number of participants.",
            ErrorCodes.EventFull
        );
      }

      _event.Users.Add(user);
      await _unitOfWork.Events.UpdateAsync(_event);
      await _unitOfWork.SaveChangesAsync();
    }
  }
}
