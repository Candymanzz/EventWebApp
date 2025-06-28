using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class FilterEventsUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public FilterEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<IEnumerable<EventDto>> ExecuteAsync(
        string? category,
        string? location,
        DateTime? dateTime,
        string? title,
        CancellationToken cancellationToken = default
    )
    {
      var events = await _unitOfWork.Events.GetByFiltersAsync(
          category,
          location,
          dateTime,
          title,
          cancellationToken
      );
      return mapper.Map<IEnumerable<EventDto>>(events);
    }
  }
}
