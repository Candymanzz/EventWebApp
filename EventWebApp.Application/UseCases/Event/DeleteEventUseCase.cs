using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class DeleteEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventUseCase(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid id)
    {
      var existingEvent = await _unitOfWork.Events.GetByIdForUpdateAsync(id);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      await _unitOfWork.Events.DeleteAsync(id);
      await _unitOfWork.SaveChangesAsync();
    }
  }
}
