using Microsoft.AspNetCore.Authorization;

namespace TTAServer
{
    internal class MinimumAgeRequirement : IAuthorizationRequirement
    {
        //public int Age { get; private set; }
        public string Age { get; private set; }

        //public MinimumAgeRequirement(int age)
        public MinimumAgeRequirement(string age)
        {
            Age = age;
        }
    }
}