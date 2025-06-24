using AutoMapper;
using EventWebApp.Application.DTOs;
using EventWebApp.Core.Interfaces;
using FluentValidation;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserUseCase
  {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IValidator<UserRegistrationRequest> validator;

    public RegisterUserUseCase(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<UserRegistrationRequest> validator
    )
    {
      this.userRepository = userRepository;
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
      await userRepository.AddAsync(user);
      return mapper.Map<UserDto>(user);
    }
  }
}
