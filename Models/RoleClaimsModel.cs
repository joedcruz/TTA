using System.ComponentModel.DataAnnotations;

namespace TTAServer
{
    /// <summary>
    /// Used by API ClaimsToAssign. Not used in this project currently
    /// </summary>
    public class RolesClaimsModel
    {
            public string Type { get; set; }

            public string Value { get; set; }
    }
}
