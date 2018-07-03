using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.BoardGameGeek;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IBGGService
    {
        BGGGame Search(string query);

        Game SearchByID(int id);
    }
}