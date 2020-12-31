using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Picro.Module.Identity.Service.Interface;
using Picro.Server.Utils;

namespace Picro.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IUserIdentityService _userIdentityService;

        private readonly string _identificationCookieName;

        public IdentityController(
            IUserIdentityService userIdentityService,
            IConfiguration configuration)
        {
            _userIdentityService = userIdentityService;
            _identificationCookieName = configuration["IdentificationCookieName"];
        }

        [HttpGet]
        [Route("BeginSession")]
        public async Task<IActionResult> BeginSession()
        {
            Guid callerId;

            if (!Request.Cookies.ContainsKey(_identificationCookieName))
            {
                // We have a new client, lets add the cookie and register him in the database
                callerId = Guid.NewGuid();

                var cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddYears(100),
                    MaxAge = DateTime.UtcNow.AddYears(100).TimeOfDay,
#if RELEASE
				SameSite = SameSiteMode.None,
				Secure = true,
#endif
                };

                Response.Cookies.Append(_identificationCookieName, callerId.ToString(), cookieOptions);

                await _userIdentityService.RegisterNewUser(callerId);

                return JsonResponse.Success();
            }
            else
            {
                callerId = Guid.Parse(Request.Cookies[_identificationCookieName]!);

                if (await _userIdentityService.IdentifyUser(callerId))
                {
                    return JsonResponse.Success();
                }
            }

            return JsonResponse.Error();
        }
    }
}