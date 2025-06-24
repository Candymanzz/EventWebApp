using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class UpdateRefreshTokenUseCase
  {
    private readonly IUserRepository userRepository;

    public UpdateRefreshTokenUseCase(IUserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    public async Task ExecuteAsync(Guid userId, string refreshToken, DateTime expiry)
    {
      var result = await userRepository.UpdateRefreshTokenAsync(userId, refreshToken, expiry);
      if (!result)
      {
        throw new NotFoundException("User not found.", ErrorCodes.UserNotFound);
      }
    }
  }
}