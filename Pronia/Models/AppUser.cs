using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Pronia.Models
{
    public class AppUser:IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<Order> Orders { get; set; }
    }
}
