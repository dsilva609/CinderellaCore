using CinderellaCore.Model.Models;
using CinderellaCore.Web.Enums;
using CinderellaCore.Web.HTMLHelpers;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using CompletionStatus = CinderellaCore.Model.CompletionStatus;

namespace CinderellaCore.Web.Controllers
{
    public class CinderellaCoreBaseController : Controller
    {
        protected ApplicationUser _user => GetCurrentUser().Result;

        protected readonly UserManager<ApplicationUser> _userManager;

        public CinderellaCoreBaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetCurrentUser() => HttpContext != null ? await _userManager.GetUserAsync(HttpContext.User) : null;

        public ToastMessage ShowStatusMessage(MessageTypeEnum toastType, string message, string title)
        {
            var toastr = TempData.Get<Toastr>("Toastr");
            toastr = toastr ?? new Toastr();

            var toastMessage = toastr.AddToastMessage(title, message, toastType);
            TempData.Add<Toastr>("Toastr", toastr);

            return toastMessage;
        }

        public void SetTimeStamps(BaseItem model)
        {
            if (model.CompletionStatus == CompletionStatus.InProgress) model.DateStarted = DateTime.UtcNow;
            else if (model.CompletionStatus == CompletionStatus.Completed) model.DateCompleted = DateTime.UtcNow;
        }

        public bool SessionValueExists(string key) => HttpContext.Session.Keys.Any(x => x == key);

        public void SetSessionString(string key, object value) => HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));

        public string GetStringFromSession(string key) => HttpContext.Session.GetString(key).Replace("\"", string.Empty);

        public T GetFromSession<T>(string key)
        {
            var value = HttpContext.Session.GetString(key);

            return string.IsNullOrWhiteSpace(value) ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public void RemoveFromSession(string key)
            => HttpContext.Session.Remove(key);
    }
}