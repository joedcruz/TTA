using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TTAServer.Controllers
{
    /// <summary>
    /// Manages the standard web server pages
    /// </summary>
    public class HomeController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected ApplicationDbContext mContext;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles, etc..
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The manager for handling signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Default comstructor
        /// </summary>
        /// <param name="context">The iinjected context</param>
        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
        }

        #endregion

        public IActionResult Index()
        {
            // Make sure we have the database
            mContext.Database.EnsureCreated();

            if (!mContext.Settings.Any())
            {
                mContext.Settings.Add(new SettingsDataModel
                {
                    Name = "BackgroundColor",
                    Value = "Black"
                });

                var settingsLocally = mContext.Settings.Local.Count();
                var settingsDatabase = mContext.Settings.Count();

                var firstLocal = mContext.Settings.Local.FirstOrDefault();
                var firstDatabase = mContext.Settings.FirstOrDefault(); // Will be null since the record is not stored to the database

                mContext.SaveChanges(); // Commit changes to the database

                settingsLocally = mContext.Settings.Local.Count();
                settingsDatabase = mContext.Settings.Count();

                firstLocal = mContext.Settings.Local.FirstOrDefault();
                firstDatabase = mContext.Settings.FirstOrDefault();
            }

            return View();
        }

        // Private area
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("api/mysecuredmethod")]
        public IActionResult MySecuredMethod()
        {
            return Content($"This is a secured method accessed by user {HttpContext.User.Identity.Name}", "text/html");
        }

        //[Route("logout")]
        //public async Task<IActionResult> SignOutAsync()
        //{
        //    await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        //    return Content("User logged out");
        //}

        /// <summary>
        /// An auto-login page for testing
        /// </summary>
        /// <param name="returnUrl">The url to return to if successfully logged in</param>
        /// <returns></returns>
        //[Route("login")]
        //public async Task<IActionResult> LoginAsync(string returnUrl)
        //{
        //    // Sign out any previous sessions
        //    await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        //    // Sign user in with valid credentials
        //    var result = await mSignInManager.PasswordSignInAsync("joedcruz", "password", true, false);

        //    if (result.Succeeded)
        //    {
        //        // If we have no return URL...
        //        if (string.IsNullOrEmpty(returnUrl))
        //            // Go to home
        //            return RedirectToAction(nameof(Index));

        //        // Otherwise, go to the return url
        //        return Redirect(returnUrl);
        //    }

        //    return Content("Failed to login", "text/html");
        //}
    }
}
