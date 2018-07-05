using CinderellaCore.Model.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinderellaCore.Web.Models
{
    public class WishFormModel
    {
        public Wish Wish { get; set; }
        public SelectList Categories { get; set; }
    }
}