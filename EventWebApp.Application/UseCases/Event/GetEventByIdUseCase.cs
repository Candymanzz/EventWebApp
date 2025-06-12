using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
    public class GetEventByIdUseCase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public GetEventByIdUseCase (IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<EventDto?> ExecuteAsync(Guid id)
        {
            var ev = await eventRepository.GetByIdAsync(id);
            return ev == null ? null : mapper.Map<EventDto>(ev);
        } 
    }
}
