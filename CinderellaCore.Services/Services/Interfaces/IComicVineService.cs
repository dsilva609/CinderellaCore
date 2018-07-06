using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.ComicVine;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IComicVineService
    {
        ComicVineResult Search(string query);

        Book SearchByID(string id);
    }
}