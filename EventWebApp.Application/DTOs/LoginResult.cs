namespace EventWebApp.Application.DTOs
{
  public class LoginResult
  {
    public bool IsSuccess { get; set; }
    public AuthResponse? AuthResponse { get; set; }
    public bool RequiresDetails { get; set; }
    public string? Email { get; set; }
    public string? ErrorMessage { get; set; }

    public static LoginResult Success(AuthResponse authResponse)
    {
      return new LoginResult
      {
        IsSuccess = true,
        AuthResponse = authResponse
      };
    }

    public static LoginResult RequiresUserDetails(string email)
    {
      return new LoginResult
      {
        IsSuccess = false,
        RequiresDetails = true,
        Email = email
      };
    }

    public static LoginResult Failure(string errorMessage)
    {
      return new LoginResult
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

      if (RequiresDetails)
      {
        return (200, new { requiresDetails = true, email = Email });
      }

      return (400, ErrorMessage!);
    }
  }
}