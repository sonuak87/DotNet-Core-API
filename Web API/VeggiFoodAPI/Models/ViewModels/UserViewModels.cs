using VeggiFoodAPI.Models.DTOs;

namespace VeggiFoodAPI.Models.ViewModels
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel : LoginModel
    {
        public string Email { get; set; }
    }

    public class UserDetails 
    {
        public string? Username { get; set; }
        public UserAddress? Address { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }

    }
}
