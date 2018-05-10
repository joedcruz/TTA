using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer.Models
{
    public class ClaimsToAssign
    {
        public string Username { get; set; }

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
