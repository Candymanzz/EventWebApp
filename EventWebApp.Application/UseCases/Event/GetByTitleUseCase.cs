using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var ev = await eventRepository.GetByTitleAsync(title);
            return ev == null ? null : mapper.Map<EventDto>(ev);
        }
    }
}
