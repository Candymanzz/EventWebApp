using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;
using FluentValidation;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserUseCase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;
    private readonly IValidator<UserRegistrationRequest> validator;

    public RegisterUserUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UserRegistrationRequest> validator
    )
    {
      this._unitOfWork = unitOfWork;
      this.mapper = mapper;
      this.validator = validator;
    }

    public async Task<UserDto> ExecuteAsync(UserRegistrationRequest request)
    {
      var result = await validator.ValidateAsync(request);
      if (!result.IsValid)
      {
        throw new ValidationException(result.Errors);
      }

      var user = mapper.Map<Core.Model.User>(request);
      await _unitOfWork.Users.AddAsync(user);
      await _unitOfWork.SaveChangesAsync();
      return mapper.Map<UserDto>(user);
    }
  }
}
