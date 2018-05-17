using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TTAServer
{
    // This class contains logic for determining whether MinimumAgeRequirements in authorizaiton
    // policies are satisfied or not
    internal class ControllerIdentityAuthorizationHandler : AuthorizationHandler<ControllerIdentityRequirement>
    {
        private readonly ILogger<ControllerIdentityAuthorizationHandler> _logger;
        private readonly IHttpContextAccessor _accessor;
        ApplicationDbContext _dbContext;

        public ControllerIdentityAuthorizationHandler(ILogger<ControllerIdentityAuthorizationHandler> logger, ApplicationDbContext dbContext, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        
        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ControllerIdentityRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            //_logger.LogWarning("Evaluating authorization requirement for age >= {age}", requirement.Age);

            // Retrieve user claims 
            //var contextClaims = context.User.Claims;
            var contextClaims = _accessor.HttpContext.User.Claims;

            if (!string.IsNullOrEmpty(_accessor.HttpContext.User.Identity.Name))
            {
                // Extract UserId from User Claims
                var userId = (contextClaims.SingleOrDefault(val => val.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
                //var userId = "ef5be01f-cc0d-4e69-b819-b77f8b2d94f3";

                // Retrieve user's roles from AspNetUserRoles table
                var userRoles = _dbContext.UserRoles.Where(aaa => aaa.UserId == userId);

                // Retrieve user's claims from the AspNetRoleClaims table
                var roleClaims = _dbContext.RoleClaims.Where(bbb => bbb.ClaimType == "Controller" && bbb.ClaimValue == requirement.ControllerName); //use requirement.age string to filter 

                bool roleMatch = false;

                // Check if any roles match
                if (_dbContext.Roles != null)
                {
                    if (_dbContext.RoleClaims != null)
                    {
                        foreach (var ccc in userRoles)
                        {
                            foreach (var ddd in roleClaims)
                            {
                                if (ccc.RoleId == ddd.RoleId)
                                {
                                    roleMatch = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                // If any role match, then authorize
                if (roleMatch == true)
                {
                    //_logger.LogInformation("Minimum age authorization requirement {age} satisfied", requirement.Age);
                    context.Succeed(requirement);
                }
            }
                        
            return Task.CompletedTask;
        }
    }
}