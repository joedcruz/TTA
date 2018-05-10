using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TTAServer.Models;
using TTAServer.Authentication;
using System.Linq;
using TTAServer.Providers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace TTAServer.Controllers
{
    //public class AuthorizeTokenAttribute : AuthorizeAttribute
    //{
    //    public AuthorizeTokenAttribute()
    //    {
    //        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    //    }
    //}
    /// <summary>
    /// Manages the Web API calls
    /// </summary>    
    public class ApiController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped application context
        /// </summary>
        protected ApplicationDbContext mContext;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles, etc..
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The manager for singing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        protected RoleManager<IdentityRole> mRoleManager;

        #endregion

        #region Constructor

        public ApiController(
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

        /// <summary>
        /// Create user
        /// </summary>
        /// <returns></returns>
        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationInfo registrationInfo)
        {
            var user = new ApplicationUser { UserName = registrationInfo.MobileNo, PhoneNumber = registrationInfo.MobileNo };

            var result = await mUserManager.CreateAsync(user, registrationInfo.Password);

            if (result.Succeeded)
                return Content("User created successfully", "text/html");

            return Content("User creation failed", "text/html");
        }

        [Route("api/login")]
        public async Task<IActionResult> LogInAsync([FromBody]LoginCredentials loginCredentials)
        {
            // Get users login information and check it is correct

            var username = loginCredentials.MobileNo;

            // Get the user details
            var user = await mUserManager.FindByNameAsync(loginCredentials.MobileNo);

            // If we failed to find a user
            if (user == null)
                return Content("Cannot find user", "text/html");

            // If we got here, we have a user
            // Let's validate the password

            // Check if password is valid
            var isValidPassword = await mUserManager.CheckPasswordAsync(user, loginCredentials.Password);

            if (!isValidPassword)
                return Content("Incorrect password", "text/html");

            // If we get here, we are valid and the user passed the correct login details

            // retrieving roles for user
            var userRoles = await mUserManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var x = userRole;
            }


            // Set our tokens claims
            var claims = new[]
            {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)
            };

            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
            //var roles = userManager.GetRolesAsync(user);
            //var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
            //var roleClaims = await GetRoleClaimsAsync(roles).ConfigureAwait(false);

            //var rolesNotExists = rolesToAssign.NewRoles.Except(userManager.GetRolesAsync(user).Select(x => x.Name)).ToArray();

            //foreach (var newRole in userRoles)
            //{
            //    claimsIdentity.AddClaim(new Claim(newRole., "b"));
            //}

            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(
                // Get the secret key from configuration
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IocContainer.Configuration["Jwt:SecretKey"])),
                // Use HS256 algorithm
                SecurityAlgorithms.HmacSha256);

            // Generate the Jwt Token
            var token = new JwtSecurityToken(
                issuer: IocContainer.Configuration["Jwt:Issuer"],
                audience: IocContainer.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(3),
                signingCredentials: credentials
                );

            // Generate and return the token to user
            // token = user.GenerateJwtToken();

            return JwtSecurityTokenHandler().WriteToken(token);
            
            //return Content(token, "text/html");                      
        }

        [Route("api/assignrolestouser")]
        [HttpPut]
        public async Task<IActionResult> AssignRolesToUser([FromBody]AssignUserRoles rolesToAssign)
        {
            var user = await mUserManager.FindByNameAsync(rolesToAssign.Username);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await mUserManager.GetRolesAsync(user);

            var rolesNotExists = rolesToAssign.NewRoles.Except(mRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {
                foreach (string newRole in rolesToAssign.NewRoles)
                {
                    if (!await mRoleManager.RoleExistsAsync(newRole))
                    {
                        await mRoleManager.CreateAsync(new IdentityRole(newRole));
                    }
                }
            }

            IdentityResult removeResult = await mUserManager.RemoveFromRolesAsync(user, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await mUserManager.AddToRolesAsync(user, rolesToAssign.NewRoles);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        //[Route("api/assignclaims")]
        //[HttpPut]
        //public async Task<IActionResult> AssignClaimsToUser([FromBody] ClaimsToAssign claimsToAssign)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await mUserManager.FindByNameAsync(claimsToAssign.Username);
            
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    foreach (ClaimsToAssign.ClaimBindingModel claimModel in claimsToAssign.NewClaims)
        //    {
        //        if (user.Claims.Any(c => c.ClaimType == claimModel.Type))
        //        {

        //            await mUserManager.RemoveClaimAsync(user, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
        //        }

        //        await mUserManager.AddClaimAsync(user, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
        //    }

        //    return Ok();
        //}


        //[AuthorizeToken]
        //[Route("api/private")]
        //public IActionResult Private()
        //{
        //    var user = HttpContext.User;
        //    return Ok(new { privateData = $"some secret for {user.Identity.Name}" });
        //}
    }
}
