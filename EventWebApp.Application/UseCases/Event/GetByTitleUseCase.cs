using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.Event
{
  public class GetByTitleUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public GetByTitleUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<EventDto?> ExecuteAsync(string title, CancellationToken cancellationToken = default)
    {
      var events = await _unitOfWork.Events.GetByTitleAsync(title, cancellationToken);
      var ev = events?.FirstOrDefault();
      return ev == null ? null : mapper.Map<EventDto>(ev);
    }
  }
}
