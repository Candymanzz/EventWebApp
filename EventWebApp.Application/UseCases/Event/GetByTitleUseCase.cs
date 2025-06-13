using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
    public class GetByTitleUseCase
    {
        private IEventRepository eventRepository;
        private IMapper mapper;

        public GetByTitleUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<EventDto?> ExecuteAsync(string title)
        {
            var events = await eventRepository.GetByTitleAsync(title);
            var ev = events?.FirstOrDefault();
            return ev == null ? null : mapper.Map<EventDto>(ev);
        }
    }
}
