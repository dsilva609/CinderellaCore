using CinderellaCore.Model.Models;
using System.Collections.Generic;

namespace CinderellaCore.Web.Models
{
    public class ShowcaseViewModel : BaseViewModel
    {
        public List<Album> Albums { get; set; }
        public List<Book> Books { get; set; }
        public List<Game> Games { get; set; }
        public List<Movie> Movies { get; set; }
        public List<FunkoPop> Pops { get; set; }
    }
}