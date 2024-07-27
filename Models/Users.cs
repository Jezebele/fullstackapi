using System.ComponentModel.DataAnnotations;

namespace FullStackAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public int? FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }

    }
}
