using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Album
{
    public interface IAlbumService
    {
        void Add(Model.Models.Album album);

        List<Model.Models.Album> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1);

        Model.Models.Album GetByID(int id, string userID);

        void Edit(Model.Models.Album album);

        void Delete(int id, string userID);

        int GetCount();

        int GetHighestQueueRank(string userID);

        List<Model.Models.Album> GetRandomAlbums(string userID, int count);

        void ClearShowcase();
    }
}