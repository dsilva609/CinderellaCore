using CinderellaCore.Data.Components.Crud;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Discogs;
using CinderellaCore.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Services.Features.Album
{
    public class AlbumService : IAlbumService
    {
        private readonly ApplicationUser _user;
        private readonly IRepository<Model.Models.Album> _repository;
        private readonly IRepository<Tracklist> _tracksRepository;
        private readonly GetEntityListComponent _getEntityListComponent;
        private readonly AddEntityComponent _addEntityComponent;
        private readonly GetEntityByIDComponent _getEntityByIDComponent;
        private readonly EditEntityComponent _editEntityComponent;
        private readonly EditEntityListComponent _editEntityListComponent;
        private readonly DeleteEntityComponent _deleteEntityComponent;

        public AlbumService(IUnitOfWork uow, ApplicationUser user)
        {
            _user = user;
            _repository = uow.GetRepository<Model.Models.Album>();
            _tracksRepository = uow.GetRepository<Tracklist>();
            _getEntityListComponent = new GetEntityListComponent();
            _addEntityComponent = new AddEntityComponent();
            _getEntityByIDComponent = new GetEntityByIDComponent();
            _editEntityComponent = new EditEntityComponent();
            _editEntityListComponent = new EditEntityListComponent();
            _deleteEntityComponent = new DeleteEntityComponent();
        }

        public void Add(Model.Models.Album album)
        {
            var existingAlbum =
                _repository.GetAll()
                    .Where(
                        x =>
                            x.UserID == album.UserID && x.Title == album.Title && x.Artist == album.Artist && x.MediaType == album.MediaType &&
                            x.DiscogsID == album.DiscogsID)
                    .ToList();
            if (existingAlbum.Any())
                throw new ApplicationException($"An existing album of {album.Artist}, {album.Title}, {album.MediaType} already exists.");

            if (!string.IsNullOrWhiteSpace(album.ImageUrl) && !album.ImageUrl.Contains("https")) album.ImageUrl = album.ImageUrl.Replace("http", "https");
            _addEntityComponent.Execute(_repository, album);
        }

        public List<Model.Models.Album> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1
            /*bool sortAscending, string sortPreference*/)
        {
            var albumList = new List<Model.Models.Album>();

            albumList = _getEntityListComponent.Execute(_repository).OrderBy(x => x.Artist).ThenBy(y => y.Title).ToList();

            if (!string.IsNullOrWhiteSpace(userID))
                albumList = albumList.Where(x => x.UserID == userID).ToList();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var currentList = new List<Model.Models.Album>();
                currentList.AddRange(albumList);
                albumList = currentList.Where(x =>
                    x.Artist.Equals(query, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Title.Equals(query, StringComparison.InvariantCultureIgnoreCase)).ToList();
                var partialMatches =
                    currentList.Where(
                        x =>
                            x.Artist.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                            x.Title.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
                albumList = albumList.Concat(partialMatches).Distinct().ToList();
            }

            if (numToTake > 0)
                albumList = albumList.Skip(numToTake * (pageNum.GetValueOrDefault() - 1)).Take(numToTake).ToList();
            //if (sortPreference == "Name")
            //{
            //    if (sortAscending)
            //        cardList = cardList.OrderBy(x => x.Name).ToList();
            //    else
            //        cardList = cardList.OrderByDescending(x => x.Name).ToList();
            //}
            //else if (sortPreference == "Rank")
            //{
            //    if (sortAscending)
            //        cardList = cardList.OrderBy(x => x.Rank).ToList();
            //    else
            //        cardList = cardList.OrderByDescending(x => x.Rank).ToList();
            //}

            return albumList;
        }

        public Model.Models.Album GetByID(int id, string userID) => _getEntityByIDComponent.Execute(_repository, id, userID);

        public void Edit(Model.Models.Album album)
        {
            if (!string.IsNullOrWhiteSpace(album.ImageUrl) && !album.ImageUrl.Contains("https")) album.ImageUrl = album.ImageUrl.Replace("http", "https");
            _editEntityComponent.Execute(_repository, album);
            //if (album.Tracklist.Count > 0)
            //  _editEntityListComponent.Execute(_tracksRepository, album.Tracklist);
        }

        public void Delete(int id, string userID) =>
            _deleteEntityComponent.Execute(_repository, id, userID);

        public int GetCount() => _repository.GetCount();

        public int GetHighestQueueRank(string userID)
        {
            var albums = GetAll(userID).Where(x => x.IsQueued).OrderByDescending(y => y.QueueRank).ToList();

            return !albums.Any() ? 0 : albums.FirstOrDefault().QueueRank;
        }

        public List<Model.Models.Album> GetRandomAlbums(string userID, int count) => GetAll(userID).GetRandom(count);

        public void ClearShowcase()
        {
            var showcasedAlbums = _repository.GetAll().Where(x => x.IsShowcased).ToList();

            foreach (var album in showcasedAlbums)
            {
                album.IsShowcased = false;
                album.DateUpdated = DateTime.UtcNow;
                Edit(album);
            }
        }
    }
}