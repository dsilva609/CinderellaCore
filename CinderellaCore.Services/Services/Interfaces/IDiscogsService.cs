using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Discogs;
using System.Collections.Generic;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IDiscogsService
    {
        List<DiscogsResult> Search(string artist, string album);

        Album GetRelease(int releaseID);
    }
}