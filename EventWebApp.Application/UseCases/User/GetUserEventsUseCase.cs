using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class GetUserEventsUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public GetUserEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<List<EventDto>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
      var events = await _unitOfWork.Events.GetEventsByUserIdAsync(userId, cancellationToken);
      return events.Select(mapper.Map<EventDto>).ToList();
    }
  }
}
