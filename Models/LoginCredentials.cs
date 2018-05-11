using System.ComponentModel.DataAnnotations;

namespace TTAServer.Models
{
    /// <summary>
    /// used by API Login
    /// </summary>
    public class LoginCredentials
    {
        // Mobile No as Username
        [Required]
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
