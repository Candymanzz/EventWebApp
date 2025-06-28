namespace EventWebApp.Application.DTOs
{
  public class RefreshTokenResult
  {
    public bool IsSuccess { get; set; }
    public AuthResponse? AuthResponse { get; set; }
    public string? ErrorMessage { get; set; }

    public static RefreshTokenResult Success(AuthResponse authResponse)
    {
      return new RefreshTokenResult
      {
        IsSuccess = true,
        AuthResponse = authResponse
      };
    }

    public static RefreshTokenResult Failure(string errorMessage)
    {
      return new RefreshTokenResult
      {
        IsSuccess = false,
        ErrorMessage = errorMessage
      };
    }

    public (int statusCode, object data) ToHttpResponse()
    {
      if (IsSuccess)
      {
        return (200, AuthResponse!);
      }

      return (401, ErrorMessage!);
    }
  }
}