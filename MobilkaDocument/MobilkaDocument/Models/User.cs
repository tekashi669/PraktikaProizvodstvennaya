namespace MobilkaDocument.Models
{
    public class User
    {
        public string Email { get; set; } = "";
        public bool IsAuthenticated { get; set; }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public User? User { get; set; }
        public string AccessToken { get; set; } = "";
    }
}