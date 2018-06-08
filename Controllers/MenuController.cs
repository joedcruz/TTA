using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer
{
    public class MenuController : Controller
    {
        #region Protected Members

        protected ApplicationDbContext dbContext;
        protected TTADbContext ttaDbContext;

        #endregion

        public MenuController(TTADbContext context)
        {
            ttaDbContext = context;
        }

        [Route("api/getwebmenu")]
        public async Task<IEnumerable<WebMenuModel>> GetWebMenu()
        {
            var result = await ttaDbContext.tblWebMenu.ToListAsync();

            return result.AsEnumerable();
        }

        //[Route("api/getwebusermenu")]
        //public async Task<IEnumerable<WebMenuModel>> GetWebUserMenu()
        //{
        //    // accept token as a parameter

        //    // first check the roles for the logged in user from the user roles table

        //    // then check the forms authorized to the roles from the role claims table

        //    // then filter the records with the forms from the menu table

        //    var result = await ttaDbContext.tblWebMenu.Where(m => m.FormId != null || m.ParentId == 0).ToListAsync();

        //    return result.AsEnumerable();
        //}
    }
}
