using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;

public class GetPagedEventsUseCase
{
    private readonly IEventRepository eventRepository;
    private readonly IMapper mapper;

    public GetPagedEventsUseCase(IEventRepository eventRepository, IMapper mapper)
    {
        this.eventRepository = eventRepository;
        this.mapper = mapper;
    }

    public async Task<PaginatedResult<EventDto>> ExecuteAsync(PaginationRequest request)
    {
        var result = await eventRepository.GetPagedAsync(request.PageNumber, request.PageSize);

        return new PaginatedResult<EventDto>
        {
            Items = mapper.Map<IEnumerable<EventDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
        };
    }
}
