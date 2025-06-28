using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.Interfaces
{
  public interface IEventValidationService
  {
    Task<(bool isValid, object? result)> ValidateSearchByTitleAsync(string title);
  }
}