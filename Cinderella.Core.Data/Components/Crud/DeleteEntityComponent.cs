using CinderellaCore.Data.Repositories;

namespace CinderellaCore.Data.Components.Crud
{
    public class DeleteEntityComponent
    {
        public void Execute<T>(IRepository<T> repo, int id, string userID) where T : class => repo.Delete(id, userID);
    }
}