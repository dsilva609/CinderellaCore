using CinderellaCore.Model.Models.ComicVine;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IComicVineService
    {
        ComicVineResult Search(string query);
    }
}