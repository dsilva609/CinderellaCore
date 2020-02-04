using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Book
{
    public interface IBookService
    {
        void Add(Model.Models.Book book);

        List<Model.Models.Book> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Model.Models.Book GetByID(int id, string userID);

        void Edit(Model.Models.Book book);

        void Delete(int id, string userID);

        int GetCount();

        int GetHighestQueueRank(string userId);
    }
}