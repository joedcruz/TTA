using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer.Controllers
{
    public class TestController1 : Controller
    {
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "P_TestController1")]
        [HttpGet]
        [Route("api/TC1Method1")]
        public IActionResult TC1Method1()
        {
            return Content($"TC1Method1 accessed by user {HttpContext.User.Identity.Name}", "text/html");
        }
    }
}
