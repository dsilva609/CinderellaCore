using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.ComicVine;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IComicVineService
    {
        Task<ComicVineResult> Search(string query);

        Task<Book> SearchByID(string id);
    }
}