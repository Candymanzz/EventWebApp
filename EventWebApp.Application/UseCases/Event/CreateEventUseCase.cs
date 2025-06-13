using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using FluentValidation;

namespace EventWebApp.Application.UseCases.Event
{
    public class CreateEventUseCase
    {
        private readonly IEventRepository eventRepository;
        private readonly IValidator<CreateEventRequest> validator;
        private readonly IMapper mapper;

        public CreateEventUseCase(
            IEventRepository eventRepository,
            IValidator<CreateEventRequest> validator,
            IMapper mapper
        )
        {
            this.eventRepository = eventRepository;
            this.validator = validator;
            this.mapper = mapper;
        }

        public async Task<EventDto> ExecuteAsync(CreateEventRequest request)
        {
            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var ev = mapper.Map<Core.Model.Event>(request);
            ev.DateTime = DateTime.SpecifyKind(ev.DateTime, DateTimeKind.Utc);
            await eventRepository.AddAsync(ev);
            return mapper.Map<EventDto>(ev);
        }
    }
}
