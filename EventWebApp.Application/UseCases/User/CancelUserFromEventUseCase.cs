﻿using EventWebApp.Application.Exceptions;
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

    public async Task ExecuteAsync(Guid userId, Guid eventId, CancellationToken cancellationToken = default)
    {
      var user = await _unitOfWork.Users.GetByIdForUpdateAsync(userId, cancellationToken);
      if (user == null)
      {
        throw new NotFoundException("User not found", ErrorCodes.NotFound);
      }

      var _event = await _unitOfWork.Events.GetByIdForUpdateAsync(eventId, cancellationToken);
      if (_event == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      var isUserRegistered = _event.Users.Any(u => u.Id == userId);
      if (!isUserRegistered)
      {
        throw new NotFoundException("User is not registered for this event", ErrorCodes.UserNotRegisteredForEvent);
      }

      _event.Users.Remove(user);
      await _unitOfWork.Events.UpdateAsync(_event, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
  }
}
