﻿using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Web.Enums;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using CompletionStatus = CinderellaCore.Model.CompletionStatus;

namespace CinderellaCore.Web.Controllers
{
    public class AlbumController : CinderellaCoreBaseController
    {
        private readonly IAlbumService _service;
        private readonly IDiscogsService _discogsService;
        private readonly IWishService _wishService;
        private const int NUM_ALBUMS_TO_GET = 25;

        public AlbumController(UserManager<ApplicationUser> userManager, IAlbumService service, IDiscogsService discogsService, IWishService wishService) : base(userManager)
        {
            _service = service;
            _discogsService = discogsService;
            _wishService = wishService;
        }

        [HttpGet]
        public IActionResult Index(string albumQuery, string filter, int? page)
        {
            if (string.IsNullOrWhiteSpace(albumQuery) && SessionValueExists("album-query"))
            {
                albumQuery = GetStringFromSession("album-query");
                RemoveFromSession("album-query");
            }
            ViewBag.Filter = (string.IsNullOrWhiteSpace(albumQuery) ? filter : albumQuery)?.Trim();

            var albums = _service.GetAll(string.Empty, ViewBag.Filter) as List<Album>;

            var viewModel = new AlbumViewModel
            {
                ViewTitle = "Albums",
                Albums = albums?.AsQueryable().ToPagedList(page ?? 1, NUM_ALBUMS_TO_GET),
                PageSize = NUM_ALBUMS_TO_GET
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = SessionValueExists("albumResult") ? GetFromSession<Album>("albumResult") : new Album { UserID = _user.Id, UserNum = _user.UserNum };
            ViewBag.Title = "Create";
            RemoveFromSession("albumResult");

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateFromSearchResult(int releaseID)
        {
            var release = await _discogsService.GetRelease(releaseID);

            release.UserID = _user.Id;
            release.UserNum = _user.UserNum;

            ViewBag.Title = "Create";
            SetSessionString("albumResult", release);

            return RedirectToAction("Create", "Album");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Album model)
        {
            //TODO: need to do user checks
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.CompletionStatus == CompletionStatus.Completed && model.TimesCompleted == 0)
                        model.TimesCompleted = 1;
                    model.DateAdded = DateTime.UtcNow;
                    SetTimeStamps(model);
                    this._service.Add(model);
                }
                catch (Exception e)
                {
                    ShowStatusMessage(MessageTypeEnum.error, e.Message, "Duplicate Album");
                    return View(model);
                }
                RemoveFromSession("album-query");

                if (SessionValueExists("wish"))
                {
                    _wishService.Delete(Convert.ToInt32(GetFromSession<string>("wishID")), _user.Id);
                    RemoveFromSession("wish");
                    RemoveFromSession("wishID");
                    ShowStatusMessage(MessageTypeEnum.info, "Wish list has been updated", "Wish list");
                }
                ShowStatusMessage(MessageTypeEnum.success, "New Album Added Successfully.", "Add Successful");
                return RedirectToAction("Index", "Album");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByID(id, _user.Id);

            ViewBag.Title = $"Edit - {model.Title}";
            if (model.UserID != _user.Id) return RedirectToAction("Details", "Album", model.ID);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id) return RedirectToAction("Details", "Album", model.ID);

            //TODO--check if id exists
            if (model.DiscogsID == 0)
            {
                ShowStatusMessage(MessageTypeEnum.error, "No ID found to update.", "Missing ID");
                return RedirectToAction("Edit", "Album", id);
            }

            var release = await _discogsService.GetRelease(model.DiscogsID);

            //TODO: does this have to be here?
            model.Artist = release.Artist;
            model.Title = release.Title;
            model.YearReleased = release.YearReleased;
            model.RecordLabel = release.RecordLabel;
            model.Genre = release.Genre;
            model.ImageUrl = release.ImageUrl;
            //model.Tracklist = release.Tracklist;
            //model.Tracklist.ForEach(x => x.AlbumID = model.ID);

            return View("Edit", model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Album model)
        {
            if (!ModelState.IsValid) return View(model);
            var existingAlbums = _service.GetAll(_user.Id);
            //TODO: update this to just use an Any() call
            if (existingAlbums.Count > 0 && existingAlbums.Any(x => x.ID != model.ID && x.Artist == model.Artist && x.Title == model.Title
                                                                    && x.MediaType == model.MediaType && x.DiscogsID == model.DiscogsID))
            {
                ShowStatusMessage(MessageTypeEnum.error,
                    $"An album of Artist: {model.Artist}, Album: {model.Title}, Media Type: {model.MediaType} already exists.", "Duplicate Record");
                return View(model);
            }

            if (model.CompletionStatus == CompletionStatus.Completed && model.TimesCompleted == 0) model.TimesCompleted = 1;
            if (model.TimesCompleted > 0) model.CompletionStatus = CompletionStatus.Completed;
            SetTimeStamps(model);
            //TODO: make sure user id is the same so as not to change other users data
            var previous = _service.GetByID(model.ID, _user.Id);
            if (model.TimesCompleted > previous.TimesCompleted)
                model.LastCompleted = model.DateCompleted;
            model.DateUpdated = DateTime.UtcNow;

            _service.Edit(model);

            UpdateGenreAndStatus(model.ID);

            ShowStatusMessage(MessageTypeEnum.success,
                $"Album of Artist: {model.Artist}, Album: {model.Title}, Media Type: {model.MediaType} updated.", "Update Successful");
            return RedirectToAction("Index", "Album");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = _service.GetByID(id, _user?.Id);
            ViewBag.Title = $"Details - {model.Title}";
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This album cannot be deleted by another user", "Delete Failure");
                return RedirectToAction("Index", "Album");
            }

            _service.Delete(id, _user.Id);

            ShowStatusMessage(MessageTypeEnum.success, "", "Album Deleted Successfully");
            return RedirectToAction("Index", "Album");
        }

        //TODO: add tests and validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Search(DiscogsSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.Artist)) searchModel.Artist = searchModel.Artist.Trim();
            if (!string.IsNullOrWhiteSpace(searchModel.AlbumName)) searchModel.AlbumName = searchModel.AlbumName.Trim();
            if (SessionValueExists("album-query")) searchModel.AlbumName = GetStringFromSession("album-query");
            if (SessionValueExists("wish")) searchModel.AlbumName = GetFromSession<string>("wish");

            if (Request.Headers["Referer"].Contains("/Album/Search") && string.IsNullOrWhiteSpace(searchModel.Artist) &&
                string.IsNullOrWhiteSpace(searchModel.AlbumName))
            {
                ShowStatusMessage(MessageTypeEnum.error, "Please enter search terms.", "Search Error");
                return View(searchModel);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Artist) || !string.IsNullOrWhiteSpace(searchModel.AlbumName))
            {
                searchModel.Results = await _discogsService.Search(searchModel.Artist, searchModel.AlbumName);
                searchModel.Results = searchModel.Results.OrderByDescending(x => x.Year).ToList();
            }
            ViewBag.Title = "Album Search";
            return View(searchModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddToShowcase(int id)
        {
            var album = _service.GetByID(id, _user.Id);
            album.IsShowcased = true;
            album.DateUpdated = DateTime.UtcNow;
            _service.Edit(album);

            ShowStatusMessage(MessageTypeEnum.info, "Album added to showcase", "Showcase");
            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RemoveFromShowcase(int id)
        {
            var album = _service.GetByID(id, _user.Id);
            album.IsShowcased = false;
            album.DateUpdated = DateTime.UtcNow;
            _service.Edit(album);

            ShowStatusMessage(MessageTypeEnum.info, "Album removed from showcase", "Showcase");
            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        [Authorize]
        [HttpGet]
        public IActionResult IncreaseCompletionCount(int id)
        {
            var album = _service.GetByID(id, _user.Id);

            if (album.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This album cannot be edited by another user.", "Edit Failure");
                return RedirectToAction("Index", "Album");
            }

            album.TimesCompleted += 1;
            album.CompletionStatus = CompletionStatus.Completed;
            album.LastCompleted = DateTime.UtcNow;
            album.DateUpdated = DateTime.UtcNow;
            _service.Edit(album);

            ShowStatusMessage(MessageTypeEnum.info, "Album was updated.", "Update");
            return RedirectToAction("Index", "Album");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DecreaseCompletionCount(int id)
        {
            var album = _service.GetByID(id, _user.Id);

            if (album.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This album cannot be edited by another user.", "Edit Failure");
                return RedirectToAction("Index", "Album");
            }

            if (album.TimesCompleted > 0) album.TimesCompleted -= 1;

            if (album.TimesCompleted == 0) album.CompletionStatus = CompletionStatus.NotStarted;
            album.DateUpdated = DateTime.UtcNow;
            _service.Edit(album);

            ShowStatusMessage(MessageTypeEnum.info, "Album was updated.", "Update");
            return RedirectToAction("Index", "Album");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddToQueue(int id)
        {
            var album = _service.GetByID(id, _user.Id);

            if (album.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This album cannot be edited by another user.", "Edit Failure");
                return RedirectToAction("Index", "Album");
            }

            if (album.IsQueued)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This album is already queued.", "Edit Failure");
                return RedirectToAction("Index", "Album");
            }

            album.IsQueued = true;
            var currentHighestRank = _service.GetHighestQueueRank(_user.Id);
            album.QueueRank = currentHighestRank + 1;

            _service.Edit(album);

            ShowStatusMessage(MessageTypeEnum.info, "Album added to queue", "Queue");
            return RedirectToAction("Index", "Queue");
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemoveFromQueue(int id)
        {
            var album = _service.GetByID(id, _user.Id);

            if (album.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This album cannot be edited by another user.", "Edit Failure");
                return RedirectToAction("Index", "Album");
            }

            album.IsQueued = false;
            album.QueueRank = 0;

            _service.Edit(album);

            ShowStatusMessage(MessageTypeEnum.info, "Album removed from queue,", "Queue");
            return RedirectToAction("Index", "Queue");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ClearShowcase()
        {
            _service.ClearShowcase();

            ShowStatusMessage(MessageTypeEnum.info, "Album showcase cleared", "Showcase");

            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        private void UpdateGenreAndStatus(int id)
        {
            var album = _service.GetByID(id, _user.Id);

            if (!string.IsNullOrWhiteSpace(album.Style)) return;
            var origGenre = album.Genre;
            if (string.IsNullOrWhiteSpace(origGenre)) return;
            else
            {
                var firstDashIndex = origGenre.IndexOf('-');
                var firstCommaIndex = origGenre.IndexOf(',');
                var genre = string.Empty;
                var style = string.Empty;

                if (firstDashIndex < firstCommaIndex && firstCommaIndex != -1 && firstDashIndex != -1)
                {
                    genre = origGenre.Substring(0, firstDashIndex);
                    style = origGenre.Substring(firstDashIndex + 1);
                }
                else if (firstCommaIndex == -1 && firstDashIndex == -1)
                {
                    return;
                }
                else if (firstCommaIndex < firstDashIndex && firstCommaIndex != -1)
                {
                    genre = origGenre.Substring(0, firstCommaIndex);
                    style = origGenre.Substring(firstCommaIndex + 1);
                }
                else if (firstDashIndex > 0 && firstCommaIndex == -1)
                {
                    genre = origGenre.Substring(0, firstDashIndex);
                    style = origGenre.Substring(firstDashIndex + 1);
                }
                else if (firstCommaIndex > 0 && firstDashIndex == -1)
                {
                    genre = origGenre.Substring(0, firstCommaIndex);
                    style = origGenre.Substring(firstCommaIndex + 1);
                }
                album.Genre = genre.Trim();
                style = style.TrimStart(',');
                album.Style = style.Trim();
                album.DateUpdated = DateTime.UtcNow;
                _service.Edit(album);
                ShowStatusMessage(MessageTypeEnum.info, $"Updated album genre to: {album.Genre}, style to: {album.Style}", "Update");
            }
        }
    }
}