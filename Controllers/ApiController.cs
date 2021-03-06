﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace TTAServer
{
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
        /// Creates a user
        /// </summary>
        /// <param name="registrationInfo">Username and Password for registration as json object. Username is mobile no</param>
        /// <returns></returns>
        [Route("api/register")]
        [HttpPost]
        // To check: does not work when CUID is set in ApplicationUser
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationInfo registrationInfo)
        {
            var user = new ApplicationUser { UserName = registrationInfo.MobileNo, PhoneNumber = registrationInfo.MobileNo };

            var result = await mUserManager.CreateAsync(user, registrationInfo.Password);

            if (result.Succeeded)
                return Content("User created successfully", "text/html");
            else
                return Content("User creation failed", "text/html");
        }

        /// <summary>
        /// Allows user to login using the login credentials. Successful login return the user token
        /// </summary>
        /// <param name="loginCredentials">Credentials are Username and Password</param>
        /// <returns></returns>
        [Route("api/login")]
        public async Task<string> LogInAsync([FromBody]LoginCredentials loginCredentials)
        {
            // Get users login information and check it is correct

            var username = loginCredentials.MobileNo;

            // Get the user details
            var user = await mUserManager.FindByNameAsync(loginCredentials.MobileNo);
            
            // If we got here, we have a user
            // Let's validate the password

            // Check if password is valid
            var isValidPassword = await mUserManager.CheckPasswordAsync(user, loginCredentials.Password);

            // If we get here, we are valid and the user passed the correct login details

            // Set our tokens claims
            var claims = new List<Claim>
            {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)              
            };

            // Add user id to the claim
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            
            // Get all the roles assigned to the user
            var userRoles = await mUserManager.GetRolesAsync(user);

            // Add each role to the claim
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await mRoleManager.FindByNameAsync(userRole);
            }
            
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

            string encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        /// <summary>
        /// Assign claims to user. If the role does not exist, it will create the role and then assign.
        /// </summary>
        /// <param name="rolesToAssign">Username and string array of roles to assign</param>
        /// <returns></returns>
        [Route("api/assignrolestouser")]
        [HttpPut]
        public async Task<IActionResult> AssignRolesToUser([FromBody]UserRolesModel rolesToAssign)
        {
            // Find the user using the Username
            var user = await mUserManager.FindByNameAsync(rolesToAssign.Username);

            // If user not found, exit
            if (user == null)
            {
                return NotFound();
            }

            // If user is found in the database, get all the roles assigned to that user
            var currentRoles = await mUserManager.GetRolesAsync(user);

            // Check if the existing roles assigned are different from the new roles to be aassigned
            var rolesNotExists = rolesToAssign.RoleNames.Except(mRoleManager.Roles.Select(x => x.Name)).ToArray();

            // If the new roles to be assigned are different from the existing roles assigned
            if (rolesNotExists.Count() > 0)
            {
                foreach (string newRole in rolesToAssign.RoleNames)
                {
                    // Check if the new role already exist in the database. If not create the new role
                    if (!await mRoleManager.RoleExistsAsync(newRole))
                    {
                        await mRoleManager.CreateAsync(new IdentityRole(newRole));
                    }
                }
            }

            // Remove all the current roles before assigning the new roles
            IdentityResult removeResult = await mUserManager.RemoveFromRolesAsync(user, currentRoles.ToArray());

            // If removing roles did not succeed, exit
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            // If removing current roles succeeded, then assign all the new roles to the user
            IdentityResult addResult = await mUserManager.AddToRolesAsync(user, rolesToAssign.RoleNames);

            // If unable to assign the role to the user, exit
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
