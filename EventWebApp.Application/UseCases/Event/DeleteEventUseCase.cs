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
            await eventRepository.DeleteAsync(id);
        }
    }
}
