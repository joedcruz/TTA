using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TTAServer
{
    public class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            // If the user does not have the Role claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "role" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var roles = context.User.FindFirst(c => c.Type == "role" && c.Issuer == requirement.Issuer);

            if ((roles.ToString() == requirement.Role))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
