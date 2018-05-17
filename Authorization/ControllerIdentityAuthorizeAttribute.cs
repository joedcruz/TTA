using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer
{
    internal class ControllerIdentityAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "ControllerIdentity";

        public ControllerIdentityAuthorizeAttribute(string controllerName)
        {
            ControllerName = controllerName;
        }

        // Get or set the ControllerName property by manipulating the underlying Policy property
        public string ControllerName
        {
            get
            {
                return default(string);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
