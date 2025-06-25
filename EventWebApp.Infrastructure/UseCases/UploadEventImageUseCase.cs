using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Infrastructure.UseCases
{
  public class UploadEventImageUseCase
  {
    private readonly IEventRepository eventRepository;

    public UploadEventImageUseCase(IEventRepository eventRepository)
    {
      this.eventRepository = eventRepository;
    }

    public async Task ExecuteAsync(Guid eventId, string relativeImagePath)
    {
      // Проверка существования события
      var existingEvent = await eventRepository.GetByIdAsync(eventId);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      // Обновление изображения
      existingEvent.ImageUrl = relativeImagePath;
      await eventRepository.UpdateAsync(existingEvent);
    }
  }
}