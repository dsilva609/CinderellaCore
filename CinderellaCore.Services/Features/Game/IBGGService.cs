using CinderellaCore.Model.Models.BoardGameGeek;

namespace CinderellaCore.Services.Features.Game

{
    public interface IBGGService
    {
        BGGGame Search(string query);

        Model.Models.Game SearchByID(int id);
    }
}