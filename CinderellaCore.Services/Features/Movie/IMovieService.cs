using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Movie
{
    public interface IMovieService
    {
        void Add(Model.Models.Movie movie);

        List<Model.Models.Movie> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Model.Models.Movie GetByID(int id, string userID);

        void Edit(Model.Models.Movie movie);

        void Delete(int id, string userID);

        int GetCount();

        int GetHighestQueueRank(string userID);
    }
}