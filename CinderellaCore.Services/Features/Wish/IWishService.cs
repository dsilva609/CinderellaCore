using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Wish
{
    public interface IWishService
    {
        void Add(Model.Models.Wish wish);

        List<Model.Models.Wish> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Model.Models.Wish GetByID(int id, string userID);

        void Edit(Model.Models.Wish wish);

        void Delete(int id, string userID);

        int GetCount();
    }
}