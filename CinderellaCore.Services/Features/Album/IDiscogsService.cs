using CinderellaCore.Model.Models.Discogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Features.Album
{
    public interface IDiscogsService
    {
        Task<List<DiscogsResult>> Search(string artist, string album);

        Task<Model.Models.Album> GetRelease(int releaseID);
    }
}