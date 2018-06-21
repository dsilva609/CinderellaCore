using System.Security.Claims;

namespace CinderellaCore.Model.Models
{
    public class ApplicationUser
    {
        public ClaimsPrincipal Current { get; set; }

        public ApplicationUser(ClaimsPrincipal principal)
        {
            Current = principal;
        }
    }
}