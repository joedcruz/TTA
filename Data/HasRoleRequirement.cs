using Microsoft.AspNetCore.Authorization;
using System;

namespace TTAServer
{
    public class HasRoleRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Role { get; }

        public HasRoleRequirement(string role, string issuer)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
            Issuer = issuer ?? throw new ArgumentException(nameof(issuer));
        }
    }
}
