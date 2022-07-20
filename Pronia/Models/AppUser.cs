using Microsoft.AspNetCore.Identity;

namespace Pronia.Models
{
    public class AppUser:IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
