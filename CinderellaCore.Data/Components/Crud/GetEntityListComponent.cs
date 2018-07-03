using CinderellaCore.Data.Repositories;
using System.Linq;

namespace CinderellaCore.Data.Components.Crud
{
    public class GetEntityListComponent
    {
        public IQueryable<T> Execute<T>(IRepository<T> repo) where T : class => repo.GetAll();
    }
}