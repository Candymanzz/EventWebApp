namespace EventWebApp.Application.DTOs
{
  public class RegisterResult
  {
    public bool IsSuccess { get; set; }
    public AuthResponse? AuthResponse { get; set; }
    public string? ErrorMessage { get; set; }

    public static RegisterResult Success(AuthResponse authResponse)
    {
      return new RegisterResult
      {
        IsSuccess = true,
        AuthResponse = authResponse
      };
    }

    public static RegisterResult Failure(string errorMessage)
    {
      return new RegisterResult
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

      return (400, ErrorMessage!);
    }
  }
}