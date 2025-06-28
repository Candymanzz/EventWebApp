using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class UploadEventImageUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public UploadEventImageUseCase(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid eventId, string relativeImagePath, CancellationToken cancellationToken = default)
    {
      var existingEvent = await _unitOfWork.Events.GetByIdForUpdateAsync(eventId, cancellationToken);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      existingEvent.ImageUrl = relativeImagePath;
      await _unitOfWork.Events.UpdateAsync(existingEvent, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
  }
}