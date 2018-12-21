using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer
{
    public class NavItem
    {
        public string displayName { get; set; }
        public bool disabled { get; set; }
        public string iconName { get; set; }
        public string route { get; set; }
        public IList<NavItem> children { get; set; }

        public NavItem()
        {
            children = new List<NavItem>();
        }
    }
}
