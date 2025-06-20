namespace SmartTaskAPI.DTOs.Auth
{
    public class RegisterRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string Role { get; set; } = "Member"; // Default role is Member
    }
}
