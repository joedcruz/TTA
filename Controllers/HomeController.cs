using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TTAServer
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

        protected RoleManager<IdentityRole> mRoleManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Default comstructor
        /// </summary>
        /// <param name="context">The iinjected context</param>
        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
            mRoleManager = roleManager;
        }

        #endregion

        public IActionResult Index()
        {
            // Make sure we have the database
            //mContext.Database.EnsureCreated();

            //if (!mContext.Settings.Any())
            //{
            //    mContext.Settings.Add(new SettingsDataModel
            //    {
            //        Name = "BackgroundColor",
            //        Value = "Black"
            //    });

            //    var settingsLocally = mContext.Settings.Local.Count(); // Retrive records from persistent memory
            //    var settingsDatabase = mContext.Settings.Count(); // Retrieve records from the database

            //    var firstLocal = mContext.Settings.Local.FirstOrDefault(); // Move to the first record in the table
            //    var firstDatabase = mContext.Settings.FirstOrDefault(); // Will be null since the record is not stored to the database

            //    mContext.SaveChanges(); // Commit changes to the database

            //    settingsLocally = mContext.Settings.Local.Count();
            //    settingsDatabase = mContext.Settings.Count();

            //    firstLocal = mContext.Settings.Local.FirstOrDefault();
            //    firstDatabase = mContext.Settings.FirstOrDefault();
            //}

            return View();
        }


        /// <summary>
        /// An auto-login page for testing
        /// </summary>
        /// <param name="returnUrl">The url to return to if successfully logged in</param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginCredentials loginCredentials)
        {
            if (ModelState.IsValid)
            {
                var result = await mSignInManager.PasswordSignInAsync(loginCredentials.MobileNo, loginCredentials.Password, false, false);
                
                if (result.Succeeded)
                {
                    //GetUserRoles();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(ErrorInvalidUser));
                }
            }

            return View(loginCredentials);
        }


        public IActionResult Manage()
        {
            return View();
        }


        public IActionResult ErrorForbidden() => View();


        public IActionResult ErrorInvalidUser() => View();


        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await mSignInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }


        // View protected with custom parameterized authorization policy
        //[ControllerIdentityAuthorize("Controller1")]
        [Route("api/controller1")]
        //[Authorize]
        public IActionResult Controller1()
        {
            return View("Page1", 50);
        }
    }
}
