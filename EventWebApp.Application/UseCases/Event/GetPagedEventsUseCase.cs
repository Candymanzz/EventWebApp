using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;
using EventWebApp.Core.Model;

public class GetPagedEventsUseCase
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper mapper;

  public GetPagedEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
  {
    this._unitOfWork = unitOfWork;
    this.mapper = mapper;
  }

  public async Task<PaginatedResult<EventDto>> ExecuteAsync(PaginationRequest request, CancellationToken cancellationToken = default)
  {
    var result = await _unitOfWork.Events.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);

    return new PaginatedResult<EventDto>
    {
      Items = mapper.Map<IEnumerable<EventDto>>(result.Items),
      TotalCount = result.TotalCount,
      PageNumber = result.PageNumber,
      PageSize = result.PageSize,
    };
  }
}
