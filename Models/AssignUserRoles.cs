using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer.Models
{
    public class AssignUserRoles
    {
        public string Username { get; set; }

        public string[] NewRoles { get; set; }
    }
}
