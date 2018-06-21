using CinderellaCore.Data.Repositories;

namespace CinderellaCore.Data.Components.Crud
{
    public class AddEntityComponent
    {
        public void Execute<T>(IRepository<T> repo, T entity) where T : class => repo.Add(entity);
    }
}