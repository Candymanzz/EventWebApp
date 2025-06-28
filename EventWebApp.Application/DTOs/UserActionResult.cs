namespace EventWebApp.Application.DTOs
{
  public class UserActionResult
  {
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public static UserActionResult Success()
    {
      return new UserActionResult
      {
        IsSuccess = true
      };
    }

    public static UserActionResult Failure(string errorMessage)
    {
      return new UserActionResult
      {
        IsSuccess = false,
        ErrorMessage = errorMessage
      };
    }

    public (int statusCode, object data) ToHttpResponse()
    {
      if (IsSuccess)
      {
        return (200, new { message = "Success" });
      }

      return (400, ErrorMessage!);
    }
  }
}