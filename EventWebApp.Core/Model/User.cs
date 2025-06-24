using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventWebApp.Core.Model
{
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<Event> Events { get; set; } = new List<Event>();

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string Role { get; set; } = "User";
  }
}
