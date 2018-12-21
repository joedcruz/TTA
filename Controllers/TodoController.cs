using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TTAServer
{
    //[Produces("application/json")]
    //[Route("api/Todo")]
    public class TodoController : Controller
    {
        private readonly TTADbContext _context;

        public TodoController(TTADbContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        //[HttpGet]
        //public IActionResult GetTodo()
        //{
        //    return Json(new { data = _context.Todo.ToList() });
        //}

        //[HttpGet]
        [Route("api/gettodo")]
        public async Task<List<Todo>> GetTodo()
        {
            List<Todo> todo = new List<Todo>();
            var result = await _context.Todo.ToListAsync();

            foreach (var todoItem in result)
            {
                todo.Add(new Todo(todoItem.todoId, todoItem.todoName));
            }

            return todo;
        }

        [AuthorizeToken]
        [Route("api/getcustomers")]
        public async Task<List<CustomerTB>> GetCustomers()
        {
            List<CustomerTB> customers = new List<CustomerTB>();
            var result = await _context.CustomerTB.ToListAsync();

            foreach (var customer in result)
            {
                
                customers.Add(new CustomerTB(customer.CustomerID, customer.Name, customer.Address, customer.Country, customer.City, customer.Phoneno));
            }

            return customers;
        }

        
        // POST: api/Todo
        //[HttpPost]
        //public async Task<IActionResult> PostTodo([FromBody] Todo todo)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (todo.todoId == 0)
        //    {
        //        _context.Todo.Add(todo);

        //        await _context.SaveChangesAsync();

        //        return Json(new { success = true, message = "Add new data success." });
        //    }
        //    else
        //    {
        //        _context.Update(todo);

        //        await _context.SaveChangesAsync();

        //        return Json(new { success = true, message = "Edit data success." });
        //    }



        //}

        // DELETE: api/Todo/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var todo = await _context.Todo.SingleOrDefaultAsync(m => m.todoId == id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Todo.Remove(todo);
        //    await _context.SaveChangesAsync();

        //    return Json(new { success = true, message = "Delete success." });
        //}

        //private bool TodoExists(int id)
        //{
        //    return _context.Todo.Any(e => e.todoId == id);
        //}
    }

}

