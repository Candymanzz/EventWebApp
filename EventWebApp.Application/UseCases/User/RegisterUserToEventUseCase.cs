using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserToEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserToEventUseCase(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid userId, Guid eventId, CancellationToken cancellationToken = default)
    {
      var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
      if (user == null)
      {
        throw new NotFoundException("User not found", ErrorCodes.UserNotFound);
      }

      var eventEntity = await _unitOfWork.Events.GetByIdForUpdateAsync(eventId, cancellationToken);
      if (eventEntity == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      var isAlreadyRegistered = await _unitOfWork.Events.IsUserRegisteredForEventAsync(userId, eventId, cancellationToken);
      if (isAlreadyRegistered)
      {
        throw new ConflictException("User is already registered for this event", ErrorCodes.UserAlreadyRegisteredForEvent);
      }

      if (eventEntity.Users.Count >= eventEntity.MaxParticipants)
      {
        throw new ConflictException("Event is full", ErrorCodes.EventFull);
      }

      eventEntity.Users.Add(user);
      await _unitOfWork.Events.UpdateAsync(eventEntity, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
  }
}
