using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TTAServer
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
                
        public IActionResult Index()
        {
            return View();
        }
    }
}
