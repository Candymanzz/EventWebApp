using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;
using FluentValidation;

namespace EventWebApp.Application.UseCases.Event
{
  public class CreateEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateEventRequest> validator;
    private readonly IMapper mapper;

    public CreateEventUseCase(
        IUnitOfWork unitOfWork,
        IValidator<CreateEventRequest> validator,
        IMapper mapper
    )
    {
      this._unitOfWork = unitOfWork;
      this.validator = validator;
      this.mapper = mapper;
    }

    public async Task<EventDto> ExecuteAsync(CreateEventRequest request, CancellationToken cancellationToken = default)
    {
      var result = await validator.ValidateAsync(request, cancellationToken);

      if (!result.IsValid)
      {
        throw new ValidationException(result.Errors);
      }

      var ev = mapper.Map<Core.Model.Event>(request);
      ev.DateTime = DateTime.SpecifyKind(ev.DateTime, DateTimeKind.Utc);
      await _unitOfWork.Events.AddAsync(ev, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
      return mapper.Map<EventDto>(ev);
    }
  }
}
