using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventWebApp.Core.Model
{
  public class Event
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime DateTime { get; set; }

    public string Location { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int MaxParticipants { get; set; }

    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

    public string ImageUrl { get; set; } = string.Empty;
  }
}
