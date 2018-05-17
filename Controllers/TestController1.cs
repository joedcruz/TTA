using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TTAServer
{
    //[Authorize]
    public class TestController1 : Controller
    {
        [HttpGet]
        [Route("api/TC1Method1")]
        
        //[ControllerIdentityAuthorize("Controller2")]
        public IActionResult TC1Method1()
        {
            return Content($"TC1Method1 accessed by user {HttpContext.User.Identity.Name}", "text/html");
        }
    }
}
