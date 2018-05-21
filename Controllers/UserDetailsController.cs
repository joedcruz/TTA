using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TTAServer
{
    /// <summary>
    /// Not used in this project currently
    /// </summary>
    public class UserDetailsController : Controller
    {
        protected UserManager<ApplicationUser> mUserManager;
        protected ApplicationDbContext _dbContext;

        public UserDetailsController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            mUserManager = userManager;
            _dbContext = dbContext;
        }

        [Route("api/UserInfo")]
        public async Task<IActionResult> GetUserInfo([FromBody] UserInfoModel userInfo)
        {
            var user = await mUserManager.FindByNameAsync(userInfo.Username);

            userInfo.UserId = user.Id;
            userInfo.Email = user.Email;
            userInfo.Phone = user.PhoneNumber;

            //return Ok(user);
            return Ok(userInfo);
        }

        [Route("api/UserRoles")]
        public string[] GetUserRoles([FromBody] UserInfoModel userInfo)
        {
            var roles = _dbContext.UserRoles.Where(aaa => aaa.UserId == userInfo.UserId);

            string[] currentRoles = new string[roles.Count()];

            var i = 0;
            foreach (var ccc in roles)
            {
                currentRoles[i] = ccc.RoleId;
                i++;
            }

            return currentRoles;
        }
    }
}