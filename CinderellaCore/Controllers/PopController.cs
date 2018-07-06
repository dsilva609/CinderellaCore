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

namespace CinderellaCore.Web.Controllers
{
    public class PopController : CinderellaCoreBaseController
    {
        private readonly IPopService _service;
        private readonly IWishService _wishService;
        private const int NUM_POPS_TO_GET = 25;

        public PopController(UserManager<ApplicationUser> userManager, IPopService service, IWishService wishService) : base(userManager)
        {
            _service = service;
            _wishService = wishService;
        }

        [HttpGet]
        public IActionResult Index(string popQuery, string filter, int? page)
        {
            if (string.IsNullOrWhiteSpace(popQuery) && SessionValueExists("pop-Query"))
            {
                popQuery = GetFromSession<string>("pop-Query");
                RemoveFromSession("pop-Query");
            }
            ViewBag.Filter = (string.IsNullOrWhiteSpace(popQuery) ? filter : popQuery)?.Trim();

            var pops = _service.GetAll(string.Empty, ViewBag.Filter) as List<FunkoPop>;

            var viewModel = new PopViewModel
            {
                ViewTitle = "Pops",
                Pops = pops?.ToPagedList(page ?? 1, NUM_POPS_TO_GET),
                PageSize = NUM_POPS_TO_GET
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new FunkoPop { UserID = _user.Id, UserNum = _user.UserNum };
            ViewBag.Title = "Create";

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FunkoPop model)
        {
            //TODO: need to do user checks
            if (!ModelState.IsValid) return View(model);
            try
            {
                model.DateAdded = DateTime.UtcNow;
                _service.Add(model);
            }
            catch (Exception e)
            {
                ShowStatusMessage(MessageTypeEnum.error, e.Message, "Duplicate Pop");

                return View(model);
            }

            if (SessionValueExists("popWish"))
            {
                _wishService.Delete(Convert.ToInt32(GetFromSession<string>("popWishID")), _user.Id);

                RemoveFromSession("popWish");
                RemoveFromSession("popWishID");
                ShowStatusMessage(MessageTypeEnum.info, "Wish list has been updated", "Wish list");
            }
            ShowStatusMessage(MessageTypeEnum.success, "New Pop Added Successfully.", "Add Successful");

            return RedirectToAction("Index", "Pop");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            ViewBag.Title = $"Edit - {model.Title}";
            if (model.UserID != _user.Id) return RedirectToAction("Details", "Pop", model.ID);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(FunkoPop model)
        {
            if (!ModelState.IsValid) return View(model);
            var existingFunkoPops = _service.GetAll(_user.Id);
            if (existingFunkoPops.Any(x => x.ID != model.ID && x.Title == model.Title && x.Series == model.Series && x.Number == model.Number))
            {
                ShowStatusMessage(MessageTypeEnum.error, $"A Pop of Name: {model.Title}, Series: {model.Series}, Line: {model.PopLine} already exists.", "Duplicate Pop");

                return View(model);
            }

            //TODO: make sure user id is the same so as not to change other users data
            model.DateUpdated = DateTime.UtcNow;
            _service.Edit(model);

            ShowStatusMessage(MessageTypeEnum.success,
                $"Pop of Name: {model.Title}, Series: {model.Series}, Line: {model.PopLine} updated.", "Update Successful");

            return RedirectToAction("Index", "Pop");
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
        public IActionResult Delete(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This pop cannot be deleted by another user", "Delete Failure");

                return RedirectToAction("Index", "Pop");
            }

            _service.Delete(id, _user.Id);

            ShowStatusMessage(MessageTypeEnum.success, "", "Pop Deleted Successfully");

            return RedirectToAction("Index", "Pop");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddToShowcase(int id)
        {
            var pop = _service.GetByID(id, _user.Id);

            if (pop.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This pop cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Showcase", _user.UserNum);
            }

            pop.IsShowcased = true;
            pop.DateUpdated = DateTime.UtcNow;

            _service.Edit(pop);

            ShowStatusMessage(MessageTypeEnum.info, "Pop added to showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", _user.UserNum);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RemoveFromShowcase(int id)
        {
            var pop = _service.GetByID(id, _user.Id);
            pop.IsShowcased = false;
            pop.DateUpdated = DateTime.UtcNow;

            _service.Edit(pop);

            ShowStatusMessage(MessageTypeEnum.info, "Pop removed from showcase", "Showcase");

            return RedirectToAction("Index", "Showcase");
        }
    }
}