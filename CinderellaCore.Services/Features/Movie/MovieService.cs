using CinderellaCore.Data.Components.Crud;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Services.Features.Movie
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationUser _user;
        private readonly IRepository<Model.Models.Movie> _repository;
        private readonly GetEntityListComponent _getEntityListComponent;
        private readonly AddEntityComponent _addEntityComponent;
        private readonly GetEntityByIDComponent _getEntityByIDComponent;
        private readonly EditEntityComponent _editEntityComponent;
        private readonly DeleteEntityComponent _deleteEntityComponent;

        public MovieService(IUnitOfWork uow, ApplicationUser user)
        {
            _user = user;
            _repository = uow.GetRepository<Model.Models.Movie>();
            _getEntityListComponent = new GetEntityListComponent();
            _addEntityComponent = new AddEntityComponent();
            _getEntityByIDComponent = new GetEntityByIDComponent();
            _editEntityComponent = new EditEntityComponent();
            _deleteEntityComponent = new DeleteEntityComponent();
        }

        public void Add(Model.Models.Movie movie)
        {
            var existingMovie = _repository.GetAll().Where(x => x.UserID == movie.UserID && x.Title == movie.Title && x.Type == movie.Type).ToList();
            if (existingMovie.Count > 0)
                throw new ApplicationException($"An existing movie of {movie.Title}, {movie.Type} already exists.");
            _addEntityComponent.Execute(_repository, movie);
        }

        public List<Model.Models.Movie> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1)
        {
            var movieList = _getEntityListComponent.Execute(_repository).OrderBy(x => x.Title).ToList();

            if (!string.IsNullOrWhiteSpace(userID))
                movieList = movieList.Where(x => x.UserID == userID).ToList();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var currentList = new List<Model.Models.Movie>();
                currentList.AddRange(movieList);
                movieList = currentList.Where(x =>
                    x.Title.Equals(query, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Director.Equals(query, StringComparison.InvariantCultureIgnoreCase)).ToList();
                var partialMatches = currentList.Where(x => x.Title.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1 || x.Director.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
                movieList = movieList.Concat(partialMatches).Distinct().ToList();
            }

            if (numToTake > 0)
                movieList = movieList.Skip(numToTake * (pageNum.GetValueOrDefault() - 1)).Take(numToTake).ToList();

            return movieList;
        }

        public Model.Models.Movie GetByID(int id, string userID) => _getEntityByIDComponent.Execute(_repository, id, userID);

        public void Edit(Model.Models.Movie movie) => _editEntityComponent.Execute(_repository, movie);

        public void Delete(int id, string userID) => _deleteEntityComponent.Execute(_repository, id, userID);

        public int GetCount() => _repository.GetCount();

        public int GetHighestQueueRank(string userID)
        {
            var movies = GetAll(userID).Where(x => x.IsQueued).OrderByDescending(y => y.QueueRank).ToList();

            return movies.Any() ? movies.FirstOrDefault().QueueRank : 0;
        }
    }
}