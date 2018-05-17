using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TTAServer
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Signin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(string userName, DateTime? birthDate, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(userName)) return BadRequest("A user name is required");

            // In a real-world application, user credentials would need validated before signing in
            var claims = new List<Claim>();
            //Add a Name claim and, if birth date was provided, a DateOfBirth claim
            claims.Add(new Claim(ClaimTypes.Name, userName));
            if (birthDate.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.DateOfBirth, birthDate.Value.ToShortDateString()));
            }

            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            claims.Add(new Claim(ClaimTypes.Role, "Client"));

            // Create user's identity and sign them in
            var identity = new ClaimsIdentity(claims, "UserSpecified");
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return Redirect(returnUrl ?? "/");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}