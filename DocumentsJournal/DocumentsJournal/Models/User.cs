using System;

namespace DocumentsJournal.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "user";
        public string Department { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public AppUser User { get; set; }
        public string AccessToken { get; set; } = "";
    }
}