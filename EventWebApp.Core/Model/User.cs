using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventWebApp.Core.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate {  get; set; } = DateTime.UtcNow;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty; // Unique!!

        public Guid EventId { get; set; }

        public virtual Event Event { get; set; } = new Event();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public int Age => DateTime.UtcNow.Year - DateOfBirth.Year - 
            (DateTime.UtcNow.Date < DateOfBirth.Date.AddYears(DateTime.UtcNow.Year - DateOfBirth.Year) ? 1 : 0);
    }
}

