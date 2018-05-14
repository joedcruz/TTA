using System.ComponentModel.DataAnnotations;

namespace TTAServer
{
    /// <summary>
    /// Used by API ClaimsToAssign. Not used in this project currently
    /// </summary>
    public class ClaimsToAssign
    {
        // Username to authenticate
        public string Username { get; set; }

        // ClaimBindingModel object with the list of new claims to be assigned to the user
        public ClaimBindingModel[] NewClaims { get; set; }

        public class ClaimBindingModel
        {
            [Required]
            [Display(Name = "Claim Type")]
            public string Type { get; set; }

            [Required]
            [Display(Name = "Claim Value")]
            public string Value { get; set; }
        }
    }
}
