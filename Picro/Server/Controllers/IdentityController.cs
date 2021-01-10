using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Picro.Module.User.Service.Interface;
using Picro.Server.Utils;

namespace Picro.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly string _identificationCookieName;

        public IdentityController(
            IUserService userService,
            IConfiguration configuration)
        {
            _userService = userService;
            _identificationCookieName = configuration["IdentificationCookieName"];
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("BeginSession")]
        public async Task<IActionResult> BeginSession()
        {
            Guid callerId;

            if (!Request.HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                // We have a new client, lets add the cookie and register him in the database
                callerId = Guid.NewGuid();

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, callerId.ToString()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddYears(100),
                        AllowRefresh = true,
                    }
                );

                await _userService.RegisterNewUser(callerId);

                return JsonResponse.Success();
            }
            else
            {
                var userId = HttpContext.User.Identity!.Name!;

                callerId = Guid.Parse(userId);

                if (await _userService.IdentifyUser(callerId))
                {
                    return JsonResponse.Success();
                }
            }

            return JsonResponse.Error();
        }
    }
}