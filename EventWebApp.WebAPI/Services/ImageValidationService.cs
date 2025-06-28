using Microsoft.AspNetCore.Http;

namespace EventWebApp.WebAPI.Services
{
  public interface IImageValidationService
  {
    Task<(bool isValid, object? result)> ValidateImageUploadAsync(IFormFile file);
  }

  public class ImageValidationService : IImageValidationService
  {
    public Task<(bool isValid, object? result)> ValidateImageUploadAsync(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return Task.FromResult((false, (object?)"No file uploaded"));
      }

      return Task.FromResult((true, (object?)null));
    }
  }
}