using CinderellaCore.Model.Models.GiantBomb;

namespace CinderellaCore.Services.Features.Game
{
    public interface IGiantBombService
    {
        GiantBombResult Search(string query);

        Model.Models.Game SearchByID(int id);
    }
}