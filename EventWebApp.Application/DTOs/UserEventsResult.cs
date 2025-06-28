using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.DTOs
{
  public class UserEventsResult
  {
    public bool IsSuccess { get; set; }
    public IEnumerable<EventDto>? Events { get; set; }
    public string? ErrorMessage { get; set; }

    public static UserEventsResult Success(IEnumerable<EventDto> events)
    {
      return new UserEventsResult
      {
        IsSuccess = true,
        Events = events
      };
    }

    public static UserEventsResult Failure(string errorMessage)
    {
      return new UserEventsResult
      {
        IsSuccess = false,
        ErrorMessage = errorMessage
      };
    }

    public (int statusCode, object data) ToHttpResponse()
    {
      if (IsSuccess)
      {
        return (200, Events!);
      }

      return (400, ErrorMessage!);
    }
  }
}