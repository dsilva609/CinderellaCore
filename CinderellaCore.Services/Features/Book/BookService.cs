using CinderellaCore.Data.Components.Crud;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Services.Features.Book
{
    public class BookService : IBookService
    {
        private readonly ApplicationUser _user;
        private readonly IRepository<Model.Models.Book> _repository;
        private readonly GetEntityListComponent _getEntityListComponent;
        private readonly AddEntityComponent _addEntityComponent;
        private readonly GetEntityByIDComponent _getEntityByIDComponent;
        private readonly EditEntityComponent _editEntityComponent;
        private readonly DeleteEntityComponent _deleteEntityComponent;

        public BookService(IUnitOfWork uow, ApplicationUser user)
        {
            _user = user;
            _repository = uow.GetRepository<Model.Models.Book>();
            _getEntityListComponent = new GetEntityListComponent();
            _addEntityComponent = new AddEntityComponent();
            _getEntityByIDComponent = new GetEntityByIDComponent();
            _editEntityComponent = new EditEntityComponent();
            _deleteEntityComponent = new DeleteEntityComponent();
        }

        public void Add(Model.Models.Book book)
        {
            var existingBook = _repository.GetAll().Where(x => x.UserID == book.UserID && x.Title == book.Title && x.Author == book.Author).ToList();
            if (existingBook.Count > 0)
                throw new ApplicationException($"An existing book of {book.Title}, {book.Author} already exists.");
            if (!string.IsNullOrWhiteSpace(book.ImageUrl) && !book.ImageUrl.Contains("https")) book.ImageUrl = book.ImageUrl.Replace("http", "https");
            _addEntityComponent.Execute(_repository, book);
        }

        public List<Model.Models.Book> GetAll(string userID = "", string query = "", int numToTake = 0, int? pageNum = 1)
        {
            var bookList = _getEntityListComponent.Execute(_repository).OrderBy(x => x.Author).ThenBy(x => x.Title).ToList();

            if (!string.IsNullOrWhiteSpace(userID))
                bookList = bookList.Where(x => x.UserID == userID).ToList();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var currentList = new List<Model.Models.Book>();
                currentList.AddRange(bookList);
                bookList = currentList.Where(x =>
                    x.Title.Equals(query, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Author.Equals(query, StringComparison.InvariantCultureIgnoreCase)).ToList();
                var partialMatches = currentList.Where(x => x.Title.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1 || x.Author.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
                bookList = bookList.Concat(partialMatches).Distinct().ToList();
            }

            if (numToTake > 0)
                bookList = bookList.Skip(numToTake * (pageNum.GetValueOrDefault() - 1)).Take(numToTake).ToList();

            return bookList;
        }

        public Model.Models.Book GetByID(int id, string userID) => _getEntityByIDComponent.Execute(_repository, id, userID);

        public void Edit(Model.Models.Book book)
        {
            if (!string.IsNullOrWhiteSpace(book.ImageUrl) && !book.ImageUrl.Contains("https")) book.ImageUrl = book.ImageUrl.Replace("http", "https");
            _editEntityComponent.Execute(_repository, book);
        }

        public void Delete(int id, string userID) => _deleteEntityComponent.Execute(_repository, id, userID);

        public int GetCount() => _repository.GetCount();

        public int GetHighestQueueRank(string userId)
        {
            var books = GetAll(userId).Where(x => x.IsQueued).OrderByDescending(y => y.QueueRank).ToList();

            return books.Any() ? books.FirstOrDefault().QueueRank : 0;
        }
    }
}