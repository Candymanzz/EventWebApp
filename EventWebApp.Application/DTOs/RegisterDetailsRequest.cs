namespace EventWebApp.Application.DTOs
{
  public class RegisterDetailsRequest
  {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Role { get; set; } = string.Empty;
  }
}
