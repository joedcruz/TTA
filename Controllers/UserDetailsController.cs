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
    public class UserDetailsController : Controller
    {
        protected UserManager<ApplicationUser> mUserManager;
        protected ApplicationDbContext _dbContext;

        public UserDetailsController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            mUserManager = userManager;
            _dbContext = dbContext;
        }
              
        /// <summary>
        /// Api to retrive user info from the aspnetusers table
        /// </summary>
        /// <returns></returns>
        [AuthorizeToken]
        [Route("api/userinfo")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel))]
        public async Task<IActionResult> GetUserInfo()
        {
            UserInfoModel userInfo = new UserInfoModel();

            string prefix = "Bearer ";

            var jwtHandler = new JwtSecurityTokenHandler();
            string jwtInput = "";

            if (Request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                string x = authToken;
                jwtInput = x.Substring(prefix.Length);
            }

            var readableToken = jwtHandler.CanReadToken(jwtInput);

            var user = "";

            if(readableToken == true)
            {
                var token = jwtHandler.ReadJwtToken(jwtInput);

                var claims = token.Claims;
                
                foreach (Claim c in claims)
                {
                    if (c.Type == ClaimTypes.NameIdentifier)
                    {
                        user = c.Value;
                        userInfo.UserId = c.Value;
                    }
                }

                var result = await mUserManager.FindByIdAsync(user);
                
                if (result != null)
                {
                    userInfo.UserId = result.Id;
                    userInfo.CUID = result.CUID;
                    userInfo.Username = result.UserName;
                }
            }

            return Ok(userInfo);
        }
        

        /// <summary>
        /// Api to retrieve user roles from aspnetuserroles table 
        /// </summary>
        /// <param name="userInfo">UserId</param>
        /// <returns>currentRoles</returns>
        [AuthorizeToken]
        [Route("api/userroles")]
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


        /// <summary>
        /// Api to retrieve role claims from the aspnetroleclaims table
        /// </summary>
        /// <param name="rClaims">claim type and claim value</param>
        /// <returns>assigned claims for a role</returns>
        [AuthorizeToken]
        [Route("api/roleclaims")]
        public string[] GetRoleClaims([FromBody] RolesClaimsModel rClaims)
        {
            var roleClaims = _dbContext.RoleClaims.Where(bbb => bbb.ClaimType == rClaims.Type && bbb.ClaimValue == rClaims.Value);

            string[] assignedClaims = new string[roleClaims.Count()];

            var i = 0;
            foreach (var ccc in roleClaims)
            {
                assignedClaims[i] = ccc.RoleId;
                i++;
            }

            return assignedClaims;
        }
    }
}