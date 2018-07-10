using Microsoft.AspNetCore.Identity;

namespace CinderellaCore.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserNum { get; set; }
        public bool EnableImport { get; set; }
        public string DisplayName { get; set; }
    }
}