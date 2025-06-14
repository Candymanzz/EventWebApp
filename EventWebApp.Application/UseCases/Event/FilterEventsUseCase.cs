﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
    public class FilterEventsUseCase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public FilterEventsUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> ExecuteAsync(
            string? category,
            string? location,
            DateTime? dateTime,
            string? title
        )
        {
            var events = await eventRepository.GetByFiltersAsync(
                category,
                location,
                dateTime,
                title
            );
            return mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}
