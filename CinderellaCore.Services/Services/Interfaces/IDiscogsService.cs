using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Discogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IDiscogsService
    {
        Task<List<DiscogsResult>> Search(string artist, string album);

        Task<Album> GetRelease(int releaseID);
    }
}