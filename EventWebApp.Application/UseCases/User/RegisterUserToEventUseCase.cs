using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class RegisterUserToEventUseCase
  {
    private readonly IUserRepository userRepository;

    public RegisterUserToEventUseCase(IUserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    public async Task ExecuteAsync(Guid uId, Guid evId)
    {
      var result = await userRepository.RegisterUserToEventAsync(uId, evId);

      switch (result)
      {
        case Core.Model.RegisterUserToEventResult.Success:
          return;
        case Core.Model.RegisterUserToEventResult.UserOrEventNotFound:
          throw new NotFoundException("User or Event not found.", ErrorCodes.NotFound);
        case Core.Model.RegisterUserToEventResult.UserAlreadyRegistered:
          throw new AlreadyExistsException(
              "User is already registered for this event.",
              ErrorCodes.UserAlreadyRegisteredForEvent
          );
        case Core.Model.RegisterUserToEventResult.EventFull:
          throw new ConflictException(
              "Event has reached maximum number of participants.",
              ErrorCodes.EventFull
          );
        default:
          throw new BadRequestException("Unknown error occurred", ErrorCodes.Unknown);
      }
    }
  }
}
