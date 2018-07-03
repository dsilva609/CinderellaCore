using CinderellaCore.Model.Models;
using System.Collections.Generic;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IPopService
    {
        void Add(FunkoPop pop);

        List<FunkoPop> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        FunkoPop GetByID(int id, string userID);

        void Edit(FunkoPop pop);

        void Delete(int id, string userID);

        int GetCount();
    }
}