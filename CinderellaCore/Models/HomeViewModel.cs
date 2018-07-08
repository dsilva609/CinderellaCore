using CinderellaCore.Model.Models;
using System.Collections.Generic;

namespace CinderellaCore.Web.Models
{
    public class HomeViewModel
    {
        public List<Album> Albums { get; set; }
        public List<Album> UpdatedAlbums { get; set; }

        public List<Book> Books { get; set; }
        public List<Book> UpdatedBooks { get; set; }

        public List<Movie> Movies { get; set; }
        public List<Movie> UpdatedMovies { get; set; }

        public List<Game> Games { get; set; }
        public List<Game> UpdatedGames { get; set; }

        public List<FunkoPop> Pops { get; set; }
        public List<FunkoPop> UpdatedPops { get; set; }

        public TimerModel RecordStoreDayTimer { get; set; }
        public TimerModel FreeComicBookDayTimer { get; set; }
    }
}