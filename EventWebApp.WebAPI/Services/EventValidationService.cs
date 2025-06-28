using EventWebApp.Application.DTOs;
using EventWebApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EventWebApp.WebAPI.Services
{
  public class EventValidationService : IEventValidationService
  {
    public Task<(bool isValid, object? result)> ValidateSearchByTitleAsync(string title)
    {
      if (string.IsNullOrWhiteSpace(title))
      {
        return Task.FromResult((false, (object?)"Search title cannot be empty"));
      }

      return Task.FromResult((true, (object?)null));
    }
  }
}