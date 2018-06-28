using System;
using System.Collections.Generic;
using CinderellaCore.Model.Models;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IAlbumService
    {
        void Add(Album album);

        List<Album> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Album GetByID(int id, string userID);

        void Edit(Album album);

        void Delete(int id, string userID);

        int GetCount();

        int GetHighestQueueRank(string userID);

        List<Album> GetRandomAlbums(string userID, int count);
    }

}
