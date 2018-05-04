using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EntityFrameworkBasics.Controllers
{
    public class HomeController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected ApplicationDbContext mContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Default comstructor
        /// </summary>
        /// <param name="context">The iinjected context</param>
        public HomeController(ApplicationDbContext context)
        {
            mContext = context;
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
    }
}
