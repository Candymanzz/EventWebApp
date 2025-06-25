using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class CancelUserFromEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public CancelUserFromEventUseCase(IUnitOfWork unitOfWork)
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

      if (!_event.Users.Contains(user))
      {
        throw new NotFoundException("User is not registered for this event", ErrorCodes.NotFound);
      }

      _event.Users.Remove(user);
      await _unitOfWork.Events.UpdateAsync(_event);
      await _unitOfWork.SaveChangesAsync();
    }
  }
}
