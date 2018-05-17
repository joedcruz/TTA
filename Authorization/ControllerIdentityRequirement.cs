using Microsoft.AspNetCore.Authorization;

namespace TTAServer
{
    internal class ControllerIdentityRequirement : IAuthorizationRequirement
    {
        public string ControllerName { get; private set; }

        public ControllerIdentityRequirement(string controllerName)
        {
            ControllerName = controllerName;
        }
    }
}