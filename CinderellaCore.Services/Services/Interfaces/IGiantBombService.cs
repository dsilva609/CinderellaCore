using System;
namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IGiantBombService
    {
        GiantBombResult Search(string query);

        Game SearchByID(int id);
    }
}
