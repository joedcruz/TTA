using Microsoft.AspNetCore.Authorization;

namespace TTAServer
{
    internal class ControllerIdentityRequirement : IAuthorizationRequirement
    {
        //public int Age { get; private set; }
        public string ControllerName { get; private set; }

        //public MinimumAgeRequirement(int age)
        public ControllerIdentityRequirement(string controllerName)
        {
            ControllerName = controllerName;
        }
    }
}