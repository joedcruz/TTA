using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer.Models
{
    public class LoginCredentials
    {
        [Required]
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
