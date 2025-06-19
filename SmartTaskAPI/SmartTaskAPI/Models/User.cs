using System.ComponentModel.DataAnnotations;

namespace SmartTaskAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } = "Member"; // "Admin", "User"
    }
}