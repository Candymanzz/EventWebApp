using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;
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

      var existingEvent = await eventRepository.GetByIdAsync(request.Id);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      var update = mapper.Map<Core.Model.Event>(request);
      update.DateTime = DateTime.SpecifyKind(update.DateTime, DateTimeKind.Utc);
      await eventRepository.UpdateAsync(update);
    }
  }
}
