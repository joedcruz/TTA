using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TTAServer.Authentication
{
    /// <summary>
    /// Extension methods for working with Jwt Bearer tokens
    /// </summary>
    public static class JwtTokenExtensionMethods
    {        
        public static string GenerateJwtToken(this ApplicationUser user)
        {
            //var mUserManager = new UserManager<ApplicationUser>(UserStore<ApplicationUser> store);

            //protected UserManager<ApplicationUser> mUserManager;

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

            // Return the generated token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
