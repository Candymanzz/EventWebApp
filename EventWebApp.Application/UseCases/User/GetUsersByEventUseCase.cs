using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class GetUsersByEventUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public GetUsersByEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> ExecuteAsync(Guid evId, CancellationToken cancellationToken = default)
    {
      var users = await _unitOfWork.Users.GetUsersByEvent(evId, cancellationToken);
      return mapper.Map<IEnumerable<UserDto>>(users);
    }
  }
}
