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

        //public CTRAuthorizeAttribute(int age)
        public ControllerIdentityAuthorizeAttribute(string controllerName)
        {
            ControllerName = controllerName;
        }

        // Get or set the Age property by manipulating the underlying Policy property
        //public int Age
        public string ControllerName
        {
            get
            {
                //if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var age))
                //{
                    //return age;
                //}
                return default(string);
            }
            set
            {
                //Policy = $"{POLICY_PREFIX}{value.ToString()}";
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
