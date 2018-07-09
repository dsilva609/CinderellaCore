using CinderellaCore.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CinderellaCore.Web.Authorization
{
    public class ImportAuthorizationHandler : AuthorizationHandler<ImportRequirement>
    {
        private readonly GlobalSettings _settings;

        public ImportAuthorizationHandler(GlobalSettings settings)
        {
            _settings = settings;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ImportRequirement requirement)
        {
            if (((AuthorizationFilterContext)context.Resource).HttpContext.Request.Headers["Authorization"] == _settings.ImportKey) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}