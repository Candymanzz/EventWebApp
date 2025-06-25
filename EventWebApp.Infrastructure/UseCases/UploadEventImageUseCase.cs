using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Infrastructure.UseCases
{
  public class UploadEventImageUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public UploadEventImageUseCase(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid eventId, string relativeImagePath)
    {
      var existingEvent = await _unitOfWork.Events.GetByIdAsync(eventId);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      existingEvent.ImageUrl = relativeImagePath;
      await _unitOfWork.Events.UpdateAsync(existingEvent);
      await _unitOfWork.SaveChangesAsync();
    }
  }
}