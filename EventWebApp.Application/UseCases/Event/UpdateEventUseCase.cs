using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;
using FluentValidation;

namespace EventWebApp.Application.UseCases.Event
{
  public class UpdateEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateEventRequest> validator;
    private readonly IMapper mapper;

    public UpdateEventUseCase(
        IUnitOfWork unitOfWork,
        IValidator<UpdateEventRequest> validator,
        IMapper mapper
    )
    {
      this._unitOfWork = unitOfWork;
      this.validator = validator;
      this.mapper = mapper;
    }

    public async Task ExecuteAsync(UpdateEventRequest request, CancellationToken cancellationToken = default)
    {
      var result = await validator.ValidateAsync(request, cancellationToken);
      if (!result.IsValid)
      {
        throw new ValidationException(result.Errors);
      }

      var existingEvent = await _unitOfWork.Events.GetByIdForUpdateAsync(request.Id, cancellationToken);
      if (existingEvent == null)
      {
        throw new NotFoundException("Event not found", ErrorCodes.EventNotFound);
      }

      var update = mapper.Map<Core.Model.Event>(request);
      update.DateTime = DateTime.SpecifyKind(update.DateTime, DateTimeKind.Utc);
      await _unitOfWork.Events.UpdateAsync(update, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
  }
}
