using System.Collections.Generic;
using System.Security.Claims;

namespace TTAServer.Providers
{
    /// <summary>
    /// Class to create and retrive user claims. Not working currently.
    /// </summary>
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {

            List<Claim> claims = new List<Claim>();

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}
