using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;

namespace EventWebApp.Application.UseCases.User
{
  public class UpdateRefreshTokenUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRefreshTokenUseCase(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(Guid userId, string? refreshToken, DateTime? expiry)
    {
      var user = await _unitOfWork.Users.GetByIdAsync(userId);
      if (user == null)
      {
        throw new NotFoundException("User not found.", ErrorCodes.UserNotFound);
      }

      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiryTime = expiry;
      await _unitOfWork.Users.UpdateAsync(user);
      await _unitOfWork.SaveChangesAsync();
    }
  }
}