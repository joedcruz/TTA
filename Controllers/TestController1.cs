using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TTAServer
{
    public class TestController1 : Controller
    {
        //[Authorize(AuthenticationSchemes = "Bearer", Policy = "P_TestController1")]
        [Authorize]
        [HttpGet]
        [Route("api/TC1Method1")]
        public IActionResult TC1Method1()
        {
            return Content($"TC1Method1 accessed by user {HttpContext.User.Identity.Name}", "text/html");
        }
    }
}
