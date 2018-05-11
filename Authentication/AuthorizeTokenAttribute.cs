using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TTAServer.Authentication
{
    public class AuthorizeTokenAttribute : AuthorizeAttribute
    {
        public AuthorizeTokenAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
