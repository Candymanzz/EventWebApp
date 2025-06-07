using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventWebApp.Core.CustomAttribute;

namespace EventWebApp.Core.Model
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [FutureDateValidation(ErrorMessage = "Event date must be in the future")]
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum participants must be at least 1")]
        public int MaxParticipants { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>(); //че надо??

        [Url]
        public string ImageUrl { get; set; } = string.Empty;

        [NotMapped]
        public bool IsFull => Users.Count >= MaxParticipants;
    }
}
