using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
    public class GetAllEventsUseCase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public GetAllEventsUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> ExecuteAsync()
        {
            var events = await eventRepository.GetAllAsync();
            return mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}
