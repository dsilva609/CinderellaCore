﻿using CinderellaCore.Data.Components.Crud;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Services.Services
{
    public class PopService : IPopService
    {
        private readonly ApplicationUser _user;
        private IRepository<FunkoPop> _repository;
        private GetEntityListComponent _getEntityListComponent;
        private AddEntityComponent _addEntityComponent;
        private GetEntityByIDComponent _getEntityByIDComponent;
        private EditEntityComponent _editEntityComponent;
        private DeleteEntityComponent _deleteEntityComponent;

        public PopService(IUnitOfWork uow, ApplicationUser user)
        {
            _user = user;
            _repository = uow.GetRepository<FunkoPop>();
            _getEntityListComponent = new GetEntityListComponent();
            _addEntityComponent = new AddEntityComponent();
            _getEntityByIDComponent = new GetEntityByIDComponent();
            _editEntityComponent = new EditEntityComponent();
            _deleteEntityComponent = new DeleteEntityComponent();
        }

        public void Add(FunkoPop pop)
        {
            var existingPops = _repository.GetAll().Any(x => x.UserID == pop.UserID && x.Title == pop.Title && x.Series == pop.Series && x.PopLine == pop.PopLine);

            if (existingPops) throw new ApplicationException($"An existing Pop of {pop.Title}, {pop.Series}, {pop.PopLine} already exists.");

            _addEntityComponent.Execute(_repository, pop);
        }

        public List<FunkoPop> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1)
        {
            var pops = _getEntityListComponent.Execute(_repository).OrderBy(x => x.Title).ThenBy(y => y.Series).ThenBy(z => z.Number).AsQueryable();

            if (!string.IsNullOrWhiteSpace(userID)) pops = pops.Where(x => x.UserID == userID);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var currentList = new List<FunkoPop>();
                currentList.AddRange(pops);

                pops = currentList.Where(x => x.Title.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1).AsQueryable();
            }

            if (numToTake > 0) pops = pops.Skip(numToTake * (pageNum.GetValueOrDefault() - 1)).Take(numToTake);

            return pops.ToList();
        }

        public FunkoPop GetByID(int id, string userID) => _getEntityByIDComponent.Execute(_repository, id, userID);

        public void Edit(FunkoPop pop) => _editEntityComponent.Execute(_repository, pop);

        public void Delete(int id, string userID) => _deleteEntityComponent.Execute(_repository, id, userID);

        public int GetCount() => _repository.GetCount();
    }
}