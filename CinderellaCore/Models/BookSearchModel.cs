using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.ComicVine;
using System.Collections.Generic;

namespace CinderellaCore.Web.Models
{
    public class BookSearchModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public List<Book> Volumes { get; set; }

        public ComicVineResult ComicsVineResult { get; set; }
    }
}