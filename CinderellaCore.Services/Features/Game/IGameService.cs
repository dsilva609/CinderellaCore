using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Game
{
    public interface IGameService
    {
        void Add(Model.Models.Game game);

        List<Model.Models.Game> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Model.Models.Game GetByID(int id, string userID);

        void Edit(Model.Models.Game game);

        void Delete(int id, string userID);

        int GetCount();

        int GetHighestQueueRank(string userID);
    }
}