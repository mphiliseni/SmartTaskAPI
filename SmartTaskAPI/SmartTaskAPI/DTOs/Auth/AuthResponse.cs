namespace SmartTaskAPI.DTOs.Auth
{
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? FullName { get; set; }
    }
}
