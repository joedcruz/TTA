using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer
{
    public class WebMenuModel
    {
        [Key]
        public int ItemId { get; set; }

        public string DisplayName { get; set; }

        public int OrderNo { get; set; }

        public int ParentId { get; set; }

        public string FormId { get; set; }

        public WebMenuModel()
        {

        }

        public WebMenuModel(int itemId, string displayName, int parentId)
        {
            this.ItemId = itemId;
            this.DisplayName = displayName;
            this.ParentId = parentId;
        }
    }
}
