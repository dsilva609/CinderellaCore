using System;
namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IBGGService
    {
        BGGGame Search(string query);

        Game SearchByID(int id);
    }
}
