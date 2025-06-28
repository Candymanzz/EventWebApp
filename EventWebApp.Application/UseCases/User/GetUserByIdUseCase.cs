using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class GetUserByIdUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public GetUserByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
    }

    public async Task<UserDto?> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
      var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
      return user == null ? null : mapper.Map<UserDto>(user);
    }
  }
}
