using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TTAServer
{
    //[Authorize(AuthenticationSchemes = "Bearer", Policy = "P_TestControllerA")]   
    public class TestController2 : Controller
    {
        [HttpGet]
        [Route("api/TC2Method1")]
        public IActionResult TC2Method1()
        {
            return Content($"TC2Method1 accessed by user {HttpContext.User.Identity.Name}", "text/html");
        }
    }
}
