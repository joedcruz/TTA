using System.ComponentModel.DataAnnotations;

namespace TTAServer.Models
{
    // Used by API Register for new user registration
    public class RegistrationInfo
    {
        // Mobile No as Username
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
