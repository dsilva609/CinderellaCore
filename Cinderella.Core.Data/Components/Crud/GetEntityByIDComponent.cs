using CinderellaCore.Data.Repositories;

namespace CinderellaCore.Data.Components.Crud
{
    public class GetEntityByIDComponent
    {
        public T Execute<T>(IRepository<T> repo, int id, string userID) where T : class => repo.GetByID(id, userID);
    }
}