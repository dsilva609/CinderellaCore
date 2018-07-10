using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Web.Enums;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using CompletionStatus = CinderellaCore.Model.CompletionStatus;

namespace CinderellaCore.Web.Controllers
{
    public class BookController : CinderellaCoreBaseController
    {
        private readonly IBookService _service;
        private readonly IWishService _wishService;
        private readonly IGoogleBookService _googleBookService;
        private readonly IComicVineService _comicVineService;
        private const int NUM_BOOKS_TO_GET = 25;

        public BookController(UserManager<ApplicationUser> userManager, IBookService service, IWishService wishService, IGoogleBookService googleBookService,
            IComicVineService comicVineService) : base(userManager)
        {
            _service = service;
            _wishService = wishService;
            _googleBookService = googleBookService;
            _comicVineService = comicVineService;
        }

        [HttpGet]
        public IActionResult Index(string bookQuery, string filter, int? page)
        {
            if (string.IsNullOrWhiteSpace(bookQuery) && SessionValueExists("book-query"))
            {
                bookQuery = GetFromSession<string>("book-query");
                RemoveFromSession("book-query");
            }
            ViewBag.Filter = (string.IsNullOrWhiteSpace(bookQuery) ? filter : bookQuery)?.Trim();
            var books = _service.GetAll(string.Empty, ViewBag.Filter) as List<Book>;
            var viewModel = new BookViewModel
            {
                ViewTitle = "Books",
                Books = books?.ToPagedList(page ?? 1, NUM_BOOKS_TO_GET),
                PageSize = NUM_BOOKS_TO_GET,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            ViewBag.Title = $"Details - {model.Title}";

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            var model = SessionValueExists("BookResult") ? GetFromSession<Book>("BookResult") : new Book { UserID = _user.Id, UserNum = _user.UserNum };
            RemoveFromSession("BookResult");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            try
            {
                if (book.CompletionStatus == CompletionStatus.Completed && book.TimesCompleted == 0)
                    book.TimesCompleted = 1;
                book.DateAdded = DateTime.UtcNow;
                SetTimeStamps(book);
                this._service.Add(book);
            }
            catch (Exception e)
            {
                ShowStatusMessage(MessageTypeEnum.error, e.Message, "Duplicate Book");

                return View(book);
            }
            RemoveFromSession("book-query");

            if (SessionValueExists("wish"))
            {
                _wishService.Delete(Convert.ToInt32(GetFromSession<string>("wishID")), _user.Id);
                RemoveFromSession("wish");
                RemoveFromSession("wishID");
                ShowStatusMessage(MessageTypeEnum.info, "Wish list has been updated", "Wish list");
            }

            ShowStatusMessage(MessageTypeEnum.success, "New Book Added Successfully.", "Add Successful");

            return RedirectToAction("Index", "Book");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            ViewBag.Title = $"Edit - {model.Title}";
            if (model.UserID != _user.Id) return RedirectToAction("Details", "Book", model.ID);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            var existingBooks = _service.GetAll(_user.Id);
            if (existingBooks.Count > 0 &&
                existingBooks.Any(x => x.ID != book.ID && x.Title == book.Title && x.Author == book.Author))
            {
                ShowStatusMessage(MessageTypeEnum.error, $"A book of Title: {book.Title}, Author: {book.Author} already exists.", "Duplicate Book");

                return View(book);
            }

            if (book.CompletionStatus == CompletionStatus.Completed && book.TimesCompleted == 0) book.TimesCompleted = 1;
            if (book.TimesCompleted > 0) book.CompletionStatus = CompletionStatus.Completed;
            SetTimeStamps(book);

            //TODO: make sure user id is the same so as not to change other users data
            var previous = _service.GetByID(book.ID, _user.Id);
            if (book.TimesCompleted > previous.TimesCompleted)
                book.LastCompleted = book.DateCompleted;
            book.DateUpdated = DateTime.UtcNow;
            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.success, $"Book of Title {book.Title}, Author: {book.Author}", "Update Successful");

            return RedirectToAction("Index", "Book");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This book cannot be deleted by another user", "Delete Failure");

                return RedirectToAction("Index", "Book");
            }

            _service.Delete(id, _user.Id);

            ShowStatusMessage(MessageTypeEnum.success, "", "Book Deleted Successfully");

            return RedirectToAction("Index", "Book");
        }

        //TODO: add tests and validation
        [Authorize]
        [HttpGet]
        public IActionResult Search(BookSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.Author)) searchModel.Author = searchModel.Author.Trim();
            if (!string.IsNullOrWhiteSpace(searchModel.Title)) searchModel.Title = searchModel.Title.Trim();
            if (SessionValueExists("book-query")) searchModel.Title = GetFromSession<string>("book-query");
            if (SessionValueExists("wish")) searchModel.Title = GetFromSession<string>("wish");

            if (Request.Headers["Referrer"].Contains("/Book/Search") && string.IsNullOrWhiteSpace(searchModel.Author) &&
                string.IsNullOrWhiteSpace(searchModel.Title))
            {
                ShowStatusMessage(MessageTypeEnum.error, "Please enter search terms.", "Search Error");

                return View(searchModel);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Author) || !string.IsNullOrWhiteSpace(searchModel.Title))
                searchModel.Volumes = _googleBookService.Search(searchModel.Author, searchModel.Title);

            //TODO: add author to search
            if (!string.IsNullOrWhiteSpace(searchModel.Title))
                searchModel.ComicsVineResult = _comicVineService.Search(searchModel.Title);

            ViewBag.Title = "Book Search";

            return View(searchModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateFromSearchModel(string id, bool isComic)
        {
            ViewBag.Title = "Create";
            var book = isComic ? _comicVineService.SearchByID(id) : _googleBookService.SearchByID(id);

            book.UserID = _user.Id;
            book.UserNum = _user.UserNum;
            SetSessionString("BookResult", book);

            return RedirectToAction("Create", "Book");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddToShowcase(int id)
        {
            var book = _service.GetByID(id, _user.Id);
            book.IsShowcased = true;
            book.DateUpdated = DateTime.UtcNow;
            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.info, "Book added to showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RemoveFromShowcase(int id)
        {
            var book = _service.GetByID(id, _user.Id);
            book.IsShowcased = false;
            book.DateUpdated = DateTime.UtcNow;
            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.info, "Book removed from showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        [Authorize]
        [HttpGet]
        public IActionResult IncreaseCompletionCount(int id)
        {
            var book = _service.GetByID(id, _user.Id);

            if (book.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This book cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Book");
            }

            book.TimesCompleted += 1;
            book.CompletionStatus = CompletionStatus.Completed;
            book.LastCompleted = DateTime.UtcNow;
            book.DateUpdated = DateTime.UtcNow;
            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.info, "Book was updated.", "Update");

            return RedirectToAction("Index", "Book");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DecreaseCompletionCount(int id)
        {
            var book = _service.GetByID(id, _user.Id);

            if (book.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This book cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Book");
            }

            if (book.TimesCompleted > 0) book.TimesCompleted -= 1;
            if (book.TimesCompleted == 0) book.CompletionStatus = CompletionStatus.NotStarted;
            book.DateUpdated = DateTime.UtcNow;
            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.info, "Book was updated.", "Update");

            return RedirectToAction("Index", "Book");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddToQueue(int id)
        {
            var book = _service.GetByID(id, _user.Id);

            if (book.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This book cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Book");
            }

            if (book.IsQueued)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This book is already queued.", "Edit Failure");

                return RedirectToAction("Index", "Book");
            }

            book.IsQueued = true;
            var currentHighestRank = _service.GetHighestQueueRank(_user.Id);
            book.QueueRank = currentHighestRank + 1;

            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.info, "Nook added to queue", "Queue");

            return RedirectToAction("Index", "Queue");
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemoveFromQueue(int id)
        {
            var book = _service.GetByID(id, _user.Id);

            if (book.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This book cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Album");
            }

            book.IsQueued = false;
            book.QueueRank = 0;

            _service.Edit(book);

            ShowStatusMessage(MessageTypeEnum.info, "Book removed from queue,", "Queue");

            return RedirectToAction("Index", "Queue");
        }
    }
}