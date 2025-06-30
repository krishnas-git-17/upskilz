using System.ComponentModel.DataAnnotations;

namespace theUpSkilzAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "user";
    }
}
