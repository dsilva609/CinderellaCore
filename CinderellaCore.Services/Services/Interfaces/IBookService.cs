using System;
using System.Collections.Generic;
using CinderellaCore.Model.Models;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IBookService
    {
        void Add(Book book);

        List<Book> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Book GetByID(int id, string userID);

        void Edit(Book book);

        void Delete(int id, string userID);

        int GetCount();

        int GetHighestQueueRank(string userId);
    }
}
