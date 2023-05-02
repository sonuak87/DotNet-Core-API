using Microsoft.AspNetCore.Identity;

namespace VeggiFoodAPI.Models.DTOs
{

    public class ApplicationUser: IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}
