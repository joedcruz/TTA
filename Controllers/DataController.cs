using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TTAServer
{
    /// <summary>
    /// Not used in this project currently
    /// </summary>
    public class DataController : Controller
    {
        protected UserManager<ApplicationUser> mUserManager;
        protected ApplicationDbContext _dbContext;
        protected TTADbContext _ttaDbContext;
        public string _menuString;

        public DataController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, TTADbContext ttaDbContext)
        {
            mUserManager = userManager;
            _dbContext = dbContext;
            _ttaDbContext = ttaDbContext;
        }

        /// <summary>
        /// Api to retrive user info from the aspnetusers table
        /// </summary>
        /// <returns></returns>
        //[AuthorizeToken]
        [Route("api/users")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel))]
        public List<UserInfoModel> GetUsers()
        {
            List<UserInfoModel> userInfo = new List<UserInfoModel>();

            var result = _dbContext.Users.ToList();
            
            if (result != null)
            {
                foreach (var user in result)
                {
                    userInfo.Add(new UserInfoModel
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        CUID = user.CUID,
                        Email = user.Email,
                        Phone = user.PhoneNumber
                    });
                }
            }

            return userInfo;
        }
    }
}