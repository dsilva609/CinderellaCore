using CinderellaCore.Data.Components.Crud;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Services.Features.Test
{
    public class TestService : ITestService
    {
        private readonly IRepository<TestObj> _repo;
        private readonly GetEntityListComponent _getEntityListComponent;
        private readonly AddEntityComponent _addEntityComponent;
        private readonly GetEntityByIDComponent _getEntityByIDComponent;
        private readonly EditEntityComponent _editEntityComponent;
        private readonly EditEntityListComponent _editEntityListComponent;
        private readonly DeleteEntityComponent _deleteEntityComponent;

        public TestService(IUnitOfWork uow)
        {
            _repo = uow.GetRepository<TestObj>();
            _getEntityListComponent = new GetEntityListComponent();
            _addEntityComponent = new AddEntityComponent();
            _getEntityByIDComponent = new GetEntityByIDComponent();
            _editEntityComponent = new EditEntityComponent();
            _editEntityListComponent = new EditEntityListComponent();
            _deleteEntityComponent = new DeleteEntityComponent();
        }

        public List<TestObj> GetAll() => _getEntityListComponent.Execute(_repo).ToList();
    }
}