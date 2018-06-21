using CinderellaCore.Data.Repositories;

namespace CinderellaCore.Data.Components.Crud
{
    public class EditEntityComponent
    {
        public void Execute<T>(IRepository<T> repo, T entity) where T : class => repo.Edit(entity);
    }
}