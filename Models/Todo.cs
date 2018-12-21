using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TTAServer
{
    public class Todo
    {
        public int todoId { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Todo Item Name")]
        public string todoName { get; set; }

        public Todo()
        {

        }

        public Todo(int id, string name)
        {
            this.todoId = id;
            this.todoName = name;
        }
    }
}
