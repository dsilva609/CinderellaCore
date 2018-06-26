using Microsoft.AspNetCore.Identity;

namespace CinderellaCore.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserNum { get; set; }
    }
}