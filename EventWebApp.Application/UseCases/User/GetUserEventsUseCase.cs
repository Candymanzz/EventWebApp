using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Model;

namespace EventWebApp.Application.UseCases.User
{
    public class GetUserEventsUseCase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public GetUserEventsUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<List<EventDto>> ExecuteAsync(Guid userId)
        {
            var events = await eventRepository.GetEventsByUserIdAsync(userId);
            return events.Select(mapper.Map<EventDto>).ToList();
        }
    }
}
