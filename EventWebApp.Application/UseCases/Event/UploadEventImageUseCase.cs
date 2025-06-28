using EventWebApp.Application.DTOs;
using EventWebApp.Application.Exceptions;
using EventWebApp.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EventWebApp.Application.UseCases.Event
{
  public class UploadEventImageWithValidationUseCase
  {
    private readonly IUnitOfWork _unitOfWork;

    public UploadEventImageWithValidationUseCase(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<UploadImageResult> ExecuteAsync(Guid eventId, IFormFile file, CancellationToken cancellationToken)
    {
      if (file == null || file.Length == 0)
      {
        return UploadImageResult.Failure("No file uploaded");
      }

      var existingEvent = await _unitOfWork.Events.GetByIdForUpdateAsync(eventId, cancellationToken);
      if (existingEvent == null)
      {
        return UploadImageResult.Failure("Event not found");
      }

      var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
      var path = Path.Combine("wwwroot", "images", "events", fileName);
      var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), path);

      using (var stream = new FileStream(absolutePath, FileMode.Create))
      {
        await file.CopyToAsync(stream, cancellationToken);
      }

      var relativePath = $"/images/events/{fileName}";
      existingEvent.ImageUrl = relativePath;
      await _unitOfWork.Events.UpdateAsync(existingEvent, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);

      return UploadImageResult.Success(relativePath);
    }
  }
}