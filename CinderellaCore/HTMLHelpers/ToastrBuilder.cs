using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinderellaCore.Web.HTMLHelpers
{
    public static class ToastrBuilder
    {
        public static IHtmlContent ShowStatusMessages(this IHtmlHelper htmlHelper, Toastr toastr)
        {
            var messageString = string.Empty;
            if (toastr != null)
            {
                messageString += $"toastr.options.closeButton = '{toastr.ShowCloseButton}'; ";
                messageString += $"toastr.options.newestOnTop = '{toastr.ShowNewestOnTop}'; ";

                return new HtmlString($"<script>$(function() {{ {messageString + GetMessage(toastr)} }});</script>");
            }

            return null;
        }

        private static string GetMessage(Toastr toastr)
        {
            var toastMessage = string.Empty;
            var optionsOverride = "{ 'positionClass': 'toast-top-center', 'progressBar': true }";
            foreach (var message in toastr.ToastMessages)
            {
                string toastTypeValue = message.ToastType.ToString("F").ToLower();

                if (message.IsSticky)
                {
                    toastMessage += "optionsOverride.timeOut = 0; ";
                    toastMessage += "optionsOverride.extendedTimeout = 0; ";
                }

                toastMessage += $"toastr['{toastTypeValue}']('{message.Message}', '{message.Title}', {optionsOverride}); ";
            }

            return toastMessage;
        }
    }
}