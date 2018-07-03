using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.GiantBomb;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IGiantBombService
    {
        GiantBombResult Search(string query);

        Game SearchByID(int id);
    }
}