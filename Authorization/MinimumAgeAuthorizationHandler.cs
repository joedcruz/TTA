using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace TTAServer
{
    // This class contains logic for determining whether MinimumAgeRequirements in authorizaiton
    // policies are satisfied or not
    internal class MinimumAgeAuthorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeAuthorizationHandler> _logger;
        ApplicationDbContext _dbContext;

        public MinimumAgeAuthorizationHandler(ILogger<MinimumAgeAuthorizationHandler> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            _logger.LogWarning("Evaluating authorization requirement for age >= {age}", requirement.Age);
           // Check the user's age
            // var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
            var dateOfBirthClaim = "01/01/2000";

            // TODO: Retrieve user roles from the database. Use a separate function to retrive roles and access it here.
            //var roles = context.User.FindAll(c => c.Type == ClaimTypes.Role);
            //string[] roleValues = new string[2];
            //int ctr = 0;
            //foreach (var cValue in roles)
            //{
            //    roleValues[ctr] = cValue.Value;
            //    ctr++;
            //}

            // Instead of the below static roles, these will be retrieved dynamically from aspnetroleclaims table. Create a new function to retrieve roleclaims

            var cl = context.User.Claims;
            var uid = (cl.SingleOrDefault(val => val.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
            //var userRoles = _dbContext.UserRoles;
            var userRoles = _dbContext.UserRoles.Where(aaa => aaa.UserId == uid);
            var roleClaims = _dbContext.RoleClaims;
            bool x = false;

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
                                x = true;
                            }
                        }
                    }
                }               
            }

            //string[] controllerRoles = new string[3];
            //controllerRoles[0] = "Admin1";
            //controllerRoles[1] = "Client1";
            //controllerRoles[2] = "Driver1";

            //// check if there is any match between rolevalues array and controller roles array. If any one role matches then flag as true

            //bool x = false;

            //foreach (var rValues in roleValues)
            //{
            //    foreach (var crValues in controllerRoles)
            //    {
            //        if (rValues == crValues)
            //        {
            //            x = true;
            //        }
            //    }
            //}

            if (x == true)
            {
                _logger.LogInformation("Minimum age authorization requirement {age} satisfied", requirement.Age);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Current user's DateOfBirth claim ({dateOfBirth}) does not satisfy the minimum age authorization requirement {age}",
                    //dateOfBirthClaim.Value,
                    dateOfBirthClaim,
                    requirement.Age);
            }

            //if (dateOfBirthClaim != null)
            //{
            //    If the user has a date of birth claim, check their age
            //   var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
            //    var age = DateTime.Now.Year - dateOfBirth.Year;
            //    if (dateOfBirth > DateTime.Now.AddYears(-age))
            //    {
            //        Adjust age if the user hasn't had a birthday yet this year
            //        age--;
            //    }

            //    If the user meets the age criterion, mark the authorization requirement succeeded
            //    if (age >= requirement.Age)
            //    {
            //        _logger.LogInformation("Minimum age authorization requirement {age} satisfied", requirement.Age);
            //        context.Succeed(requirement);
            //    }
            //    else
            //    {
            //        _logger.LogInformation("Current user's DateOfBirth claim ({dateOfBirth}) does not satisfy the minimum age authorization requirement {age}",
            //            dateOfBirthClaim.Value,
            //            requirement.Age);
            //    }
            //}
            //else
            //{
            //    _logger.LogInformation("No DateOfBirth claim present");
            //}

            return Task.CompletedTask;
        }
    }
}