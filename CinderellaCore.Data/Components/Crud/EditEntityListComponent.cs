using CinderellaCore.Data.Repositories;
using System.Collections.Generic;

namespace CinderellaCore.Data.Components.Crud
{
    public class EditEntityListComponent
    {
        public void Execute<T>(IRepository<T> repo, List<T> entities) where T : class => repo.Edit(entities);
    }
}