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

    public async Task ExecuteAsync(Guid userId, string? refreshToken, DateTime? expiry)
    {
      var user = await userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        throw new NotFoundException("User not found.", ErrorCodes.UserNotFound);
      }

      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiryTime = expiry;
      await userRepository.UpdateAsync(user);
    }
  }
}