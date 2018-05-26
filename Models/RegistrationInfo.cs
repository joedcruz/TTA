using System.ComponentModel.DataAnnotations;

namespace TTAServer
{
    // Used by API Register for new user registration
    public class RegistrationInfo
    {
        [Required]
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
