using CinderellaCore.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CinderellaCore.Web.Authorization
{
    public class ApiAuthorizationHandler : AuthorizationHandler<ApiRequirement>
    {
        private readonly GlobalSettings _settings;

        public ApiAuthorizationHandler(GlobalSettings settings)
        {
            _settings = settings;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiRequirement requirement)
        {
            if (((AuthorizationFilterContext)context.Resource).HttpContext.Request.Headers["Authorization"] == _settings.ApiKey) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}