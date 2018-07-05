using CinderellaCore.Model.Enums;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Web.Enums;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Web.Controllers
{
    public class WishController : CinderellaCoreBaseController
    {
        private readonly IWishService _service;
        private const int NUM_WISHES_TO_GET = 25;

        public WishController(UserManager<ApplicationUser> userManager, IWishService service) : base(userManager)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(string wishQuery, string filter, int? page)
        {
            if (string.IsNullOrWhiteSpace(wishQuery) && SessionValueExists("query"))
            {
                wishQuery = GetFromSession<string>("query");
                RemoveFromSession("query");
            }
            ViewBag.Filter = (string.IsNullOrWhiteSpace(wishQuery) ? filter : wishQuery)?.Trim();

            var wishes = _service.GetAll(_user.Id, ViewBag.Filter) as List<Wish>;

            var viewModel = new WishViewModel
            {
                ViewTitle = "Wish List",
                AlbumWishes = wishes?.Where(x => x.ItemType == ItemType.Album).ToList()?.GroupBy(y => y.Category)?.ToDictionary(d => string.IsNullOrWhiteSpace(d.Key) ? string.Empty : d.Key, d => d.ToList()),
                BookWishes = wishes?.Where(x => x.ItemType == ItemType.Book).ToList()?.GroupBy(y => y.Category)?.ToDictionary(d => string.IsNullOrWhiteSpace(d.Key) ? string.Empty : d.Key, d => d.ToList()),
                MovieWishes = wishes?.Where(x => x.ItemType == ItemType.Movie).ToList()?.GroupBy(y => y.Category)?.ToDictionary(d => string.IsNullOrWhiteSpace(d.Key) ? string.Empty : d.Key, d => d.ToList()),
                GameWishes = wishes?.Where(x => x.ItemType == ItemType.Game).ToList()?.GroupBy(y => y.Category)?.ToDictionary(d => string.IsNullOrWhiteSpace(d.Key) ? string.Empty : d.Key, d => d.ToList()),
                PopWishes = wishes?.Where(x => x.ItemType == ItemType.Pop).ToList()?.GroupBy(y => y.Category)?.ToDictionary(d => string.IsNullOrWhiteSpace(d.Key) ? string.Empty : d.Key, d => d.ToList()),
                PageSize = NUM_WISHES_TO_GET
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new WishFormModel
            {
                Wish = new Wish { UserID = _user.Id },
                Categories =
                    new SelectList(_service.GetAll(_user.Id).OrderBy(z => z.ItemType).GroupBy(x => new { x.ItemType, x.Category }).Select(y => y.First()), "Category", "Category", string.Empty, "ItemType")
            };
            ViewBag.Title = "Create";

            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(WishFormModel model)
        {
            //TODO: need to do user checks
            if (ModelState.IsValid)
            {
                try
                {
                    model.Wish.DateAdded = DateTime.UtcNow;
                    this._service.Add(model.Wish);
                }
                catch (Exception e)
                {
                    ShowStatusMessage(MessageTypeEnum.error, e.Message, "Duplicate Wish");
                    return View(model);
                }

                ShowStatusMessage(MessageTypeEnum.success, "New Wish Added Successfully.", "Add Successful");
                return RedirectToAction("Index", "Wish");
            }

            model.Categories =
                new SelectList(
                    _service.GetAll(_user.Id)
                        .OrderBy(z => z.ItemType)
                        .GroupBy(x => new { x.ItemType, x.Category })
                        .Select(y => y.First()), "Category", "Category", string.Empty, "ItemType");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var wish = _service.GetByID(id, _user.Id);

            if (wish.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This wish cannot be edited by another user.", "Edit Failure");
                return RedirectToAction("Index", "Wish");
            }
            var model = new WishFormModel
            {
                Categories =
                    new SelectList(
                        _service.GetAll(_user.Id)
                            .OrderBy(z => z.ItemType)
                            .GroupBy(x => new { x.ItemType, x.Category })
                            .Select(y => y.First()), "Category", "Category", string.Empty, "ItemType"),
                //_service.GetAll(_user.Id).Where(x => !string.IsNullOrWhiteSpace(x.Category)).Select(y => new SelectListItem
                //{
                //	Group = new SelectListGroup { Name = y.ItemType.ToString() },
                //	Text = y.Category,
                //	Value = y.Category,
                //	Selected = wish.Category == y.Category
                //}).OrderBy(z => z.Group.Name).ToList(),
                Wish = wish
            };

            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(WishFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories =
                    new SelectList(
                        _service.GetAll(_user.Id)
                            .OrderBy(z => z.ItemType)
                            .GroupBy(x => new { x.ItemType, x.Category })
                            .Select(y => y.First()), "Category", "Category", string.Empty, "ItemType");
                //, string.Empty, string.Empty,

                return View(model);
            }
            var existingWishes = _service.GetAll(_user.Id);

            if (existingWishes.Any(x => x.ID != model.Wish.ID && x.Title == model.Wish.Title && x.ItemType == model.Wish.ItemType))
            {
                ShowStatusMessage(MessageTypeEnum.error,
                    $"An wish of Title: {model.Wish.Title} and Type: {model.Wish.ItemType.ToString()} already exists.",
                    "Duplicate Record");
                model.Categories =
                    new SelectList(
                        _service.GetAll(_user.Id)
                            .OrderBy(z => z.ItemType)
                            .GroupBy(x => new { x.ItemType, x.Category })
                            .Select(y => y.First()), "Category", "Category", string.Empty, "ItemType");
                return View(model);
            }

            //TODO: make sure user id is the same so as not to change other users data
            if (model.Wish.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This wish cannot be edited by another user.", "Edit Failure");
                return RedirectToAction("Index", "Wish");
            }

            model.Wish.DateModified = DateTime.UtcNow;
            _service.Edit(model.Wish);

            ShowStatusMessage(MessageTypeEnum.success, "Wish updated.", "Update Successful");
            return RedirectToAction("Index", "Wish");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            var model = _service.GetByID(id, _user.Id);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This wish cannot be deleted by another user", "Delete Failure");
                return RedirectToAction("Index", "Wish");
            }

            _service.Delete(id, _user.Id);

            ShowStatusMessage(MessageTypeEnum.success, string.Empty, "Wish Deleted Successfully");
            return RedirectToAction("Index", "Wish");
        }

        [Authorize]
        [HttpGet]
        public IActionResult FinishWish(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This wish cannot be edited by another user", "Edit Failure");
                return RedirectToAction("Index", "Wish");
            }
            model.Owned = true;
            model.DateModified = DateTime.UtcNow;

            _service.Edit(model);

            ShowStatusMessage(MessageTypeEnum.success, string.Empty, "Wish Completed");
            return RedirectToAction("Index", "Wish");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            SetSessionString("wish", model.Title);
            SetSessionString("wishID", model.ID);

            return RedirectToAction("Search", model.ItemType.ToString());
        }
    }
}