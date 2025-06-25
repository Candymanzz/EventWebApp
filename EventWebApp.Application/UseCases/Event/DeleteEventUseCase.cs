using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class DeleteEventUseCase
  {
    private readonly IEventRepository eventRepository;

    public DeleteEventUseCase(IEventRepository eventRepository)
    {
      this.eventRepository = eventRepository;
    }

    public async Task ExecuteAsync(Guid id)
    {
      // Проверка существования события
      var existingEvent = await eventRepository.GetByIdAsync(id);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      // Удаление готовой сущности
      await eventRepository.DeleteAsync(id);
    }
  }
}
