using System.Collections.Generic;
using System.Security.Claims;

namespace TTAServer
{
    /// <summary>
    /// Class to create and retrive user claims. Not used in this project currently.
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
