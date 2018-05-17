using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TTAServer
{
    //[Authorize]
    public class TestController1 : Controller
    {
        [HttpGet]
        [Route("api/TC1Method1")]


        [Authorize(AuthenticationSchemes = "Bearer")]
        [ControllerIdentityAuthorize("Controller1")]
        public IActionResult TC1Method1()
        {
            return Content($"TC1Method1 accessed by user {HttpContext.User.Identity.Name}", "text/html");
            //var contextClaims = HttpContext.User.Claims;

            // Extract UserId from User Claims
            //var userId = (contextClaims.SingleOrDefault(val => val.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;

            //return Content($"TC1Method1 accessed by user {userId}", "text/html");
        }
    }
}
