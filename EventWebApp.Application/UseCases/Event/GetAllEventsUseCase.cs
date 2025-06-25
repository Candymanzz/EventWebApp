using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class GetAllEventsUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public GetAllEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<IEnumerable<EventDto>> ExecuteAsync()
    {
      var events = await _unitOfWork.Events.GetAllAsync();
      return mapper.Map<IEnumerable<EventDto>>(events);
    }
  }
}
