using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.DTOs
{
  public class SearchResult
  {
    public bool IsSuccess { get; set; }
    public IEnumerable<EventDto>? Events { get; set; }
    public string? ErrorMessage { get; set; }

    public static SearchResult Success(IEnumerable<EventDto> events)
    {
      return new SearchResult
      {
        IsSuccess = true,
        Events = events
      };
    }

    public static SearchResult Failure(string errorMessage)
    {
      return new SearchResult
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