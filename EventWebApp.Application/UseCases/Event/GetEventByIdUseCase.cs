using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class GetEventByIdUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public GetEventByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<EventDto?> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
      var ev = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);
      return ev == null ? null : mapper.Map<EventDto>(ev);
    }
  }
}
