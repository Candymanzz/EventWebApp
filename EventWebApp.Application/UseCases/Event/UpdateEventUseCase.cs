using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using FluentValidation;

namespace EventWebApp.Application.UseCases.Event
{
    public class UpdateEventUseCase
    {
        private readonly IEventRepository eventRepository;
        private readonly IValidator<UpdateEventRequest> validator;
        private readonly IMapper mapper;

        public UpdateEventUseCase(
            IEventRepository eventRepository,
            IValidator<UpdateEventRequest> validator,
            IMapper mapper
        )
        {
            this.eventRepository = eventRepository;
            this.validator = validator;
            this.mapper = mapper;
        }

        public async Task ExecuteAsync(UpdateEventRequest request)
        {
            var result = await validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var update = mapper.Map<Core.Model.Event>(request);
            update.DateTime = DateTime.SpecifyKind(update.DateTime, DateTimeKind.Utc);
            await eventRepository.UpdateAsync(update);
        }
    }
}
