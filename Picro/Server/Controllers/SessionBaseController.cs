using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.Service.Interface;

namespace Picro.Server.Controllers
{
    [ApiController]
    public abstract class SessionBaseController : Controller
    {
        private readonly string _identificationCookieName;

        private readonly IUserService _userService;

        protected User? UserOrNull { get; private set; }

        protected User User
        {
            get
            {
                if (UserOrNull != null)
                {
                    return UserOrNull;
                }
                throw new NullReferenceException("User not initialized");
            }
        }

        protected SessionBaseController(
            IUserService userService,
            IConfiguration configuration)
        {
            _identificationCookieName = configuration["IdentificationCookieName"];
            _userService = userService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await InitUser();
            await base.OnActionExecutionAsync(context, next);
        }

        private async Task InitUser()
        {
            var callerId = Guid.Parse(Request.Cookies[_identificationCookieName]!);
            UserOrNull = await _userService.GetUser(callerId);
        }
    }
}