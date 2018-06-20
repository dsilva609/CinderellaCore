using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CinderellaCore.Data.Components.Crud;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;

namespace CinderellaCore.Services.Services
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