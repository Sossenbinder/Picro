using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Picro.Module.User.DataTypes;
using Picro.Module.User.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Picro.Server.Controllers
{
    [ApiController]
    public abstract class SessionBaseController : Controller
    {
        private readonly IUserService _userService;

        protected PicroUser? UserOrNull { get; private set; }

        protected new PicroUser User
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

        protected SessionBaseController(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await InitUser();
            await base.OnActionExecutionAsync(context, next);
        }

        private async Task InitUser()
        {
            var userId = HttpContext.User.Identity?.Name;

            if (userId == null)
            {
                return;
            }

            UserOrNull = await _userService.GetUser(Guid.Parse(userId));
        }
    }
}