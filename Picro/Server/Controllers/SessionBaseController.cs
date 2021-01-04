﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Picro.Server.Controllers
{
    [ApiController]
    public abstract class SessionBaseController : Controller
    {
        private readonly IUserService _userService;

        protected User? UserOrNull { get; private set; }

        protected new User User
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