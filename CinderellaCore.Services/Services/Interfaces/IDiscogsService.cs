using System;
namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IDiscogsService
    {
        List<DiscogsResult> Search(string artist, string album);

        Album GetRelease(int releaseID);
    }
}
