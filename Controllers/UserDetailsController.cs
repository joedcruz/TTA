using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TTAServer
{
    /// <summary>
    /// Not used in this project currently
    /// </summary>
    public class UserDetailsController : Controller
    {
        protected UserManager<ApplicationUser> mUserManager;
        protected ApplicationDbContext _dbContext;
        protected TTADbContext _ttaDbContext;
        public string _menuString;

        public UserDetailsController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, TTADbContext ttaDbContext)
        {
            mUserManager = userManager;
            _dbContext = dbContext;
            _ttaDbContext = ttaDbContext;
        }
        
        /// <summary>
        /// Api to retrive user info from the aspnetusers table
        /// </summary>
        /// <returns></returns>
        [AuthorizeToken]
        [Route("api/userinfo")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel))]
        public async Task<IActionResult> GetUserInfo()
        {
            UserInfoModel userInfo = new UserInfoModel();

            string prefix = "Bearer ";

            var jwtHandler = new JwtSecurityTokenHandler();
            string jwtInput = "";

            if (Request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                string x = authToken;
                jwtInput = x.Substring(prefix.Length);
            }

            var readableToken = jwtHandler.CanReadToken(jwtInput);

            var user = "";

            if (readableToken == true)
            {
                var token = jwtHandler.ReadJwtToken(jwtInput);

                var claims = token.Claims;

                foreach (Claim c in claims)
                {
                    if (c.Type == ClaimTypes.NameIdentifier)
                    {
                        user = c.Value;
                        userInfo.UserId = c.Value;
                    }
                }

                var result = await mUserManager.FindByIdAsync(user);

                if (result != null)
                {
                    userInfo.UserId = result.Id;
                    userInfo.CUID = result.CUID;
                    userInfo.Username = result.UserName;
                }
            }

            return Ok(userInfo);
        }


        /// <summary>
        /// Api to retrieve user roles from aspnetuserroles table 
        /// </summary>
        /// <param name="userInfo">UserId</param>
        /// <returns>currentRoles</returns>
        [AuthorizeToken]
        [Route("api/userroles")]
        public string[] GetUserRoles([FromBody] UserInfoModel userInfo)
        {
            var roles = _dbContext.UserRoles.Where(aaa => aaa.UserId == userInfo.UserId);

            string[] currentRoles = new string[roles.Count()];

            var i = 0;
            foreach (var ccc in roles)
            {
                currentRoles[i] = ccc.RoleId;
                i++;
            }

            return currentRoles;
        }


        /// <summary>
        /// Api to retrieve role claims from the aspnetroleclaims table
        /// </summary>
        /// <param name="rClaims">claim type and claim value</param>
        /// <returns>assigned claims for a role</returns>
        [AuthorizeToken]
        [Route("api/roleclaims")]
        public string[] GetRoleClaims([FromBody] RolesClaimsModel rClaims)
        {
            var roleClaims = _dbContext.RoleClaims.Where(bbb => bbb.ClaimType == rClaims.Type && bbb.ClaimValue == rClaims.Value);
            
            string[] assignedClaims = new string[roleClaims.Count()];

            var i = 0;
            foreach (var ccc in roleClaims)
            {
                assignedClaims[i] = ccc.RoleId;
                i++;
            }

            return assignedClaims;
        }


        [AuthorizeToken]
        [Route("api/getwebusermenu")]
        public List<WebMenuModel> GetWebUserMenu([FromBody] UserInfoModel userInfo)
        //public string GetWebUserMenu([FromBody] UserInfoModel userInfo)
        //public IActionResult GetWebUserMenu([FromBody] UserInfoModel userInfo)
        //public List<NavItem> GetWebUserMenu([FromBody] UserInfoModel userInfo)
        {
            // Select all the roles assigned to the user
            var roles = _dbContext.UserRoles.Where(aaa => aaa.UserId == userInfo.UserId);

            // Select all the role claims where claim type is a Form
            var roleClaims = _dbContext.RoleClaims.Where(bbb => bbb.ClaimType == "Form");



            // string[] assignedClaims = new string[roleClaims.Count()];
            List<string> assignedClaims = new List<string>();


            //var i = 0;
            foreach (var ccc in roles)
            {
                foreach (var ddd in roleClaims)
                {
                    if (ccc.RoleId == ddd.RoleId)
                    {
                        if (assignedClaims == null)
                        {
                            assignedClaims.Add(ddd.ClaimValue);
                            //i++;
                        }
                        else 
                        {
                            if (!assignedClaims.Contains(ddd.ClaimValue))
                            {
                                assignedClaims.Add(ddd.ClaimValue);
                                //i++;
                            }
                        }
                    }
                }
            }

            // Select all menu options for the role
            var menuOptions = _ttaDbContext.tblWebMenu;

            List<WebMenuModel> webMenu = new List<WebMenuModel>();
            
            foreach (var eee in menuOptions)
            {
                foreach (var fff in assignedClaims)
                {
                    if (eee.FormId == fff.ToString())
                    {
                        webMenu.Add(new WebMenuModel
                        {
                            ItemId = eee.ItemId,
                            DisplayName = eee.DisplayName,
                            ParentId = eee.ParentId,
                            OrderNo = eee.OrderNo,
                            FormId = eee.FormId
                        });

                        int parentId = eee.ParentId;
                        foreach (var ggg in menuOptions.OrderByDescending(ord => ord.ItemId))
                        {
                            if (ggg.ItemId == parentId && ggg.ParentId >= 0)
                            {
                                if (!webMenu.Any(item => item.ItemId == ggg.ItemId))
                                {
                                    parentId = ggg.ParentId;
                                    webMenu.Add(new WebMenuModel
                                    {
                                        ItemId = ggg.ItemId,
                                        DisplayName = ggg.DisplayName,
                                        ParentId = ggg.ParentId,
                                        OrderNo = ggg.OrderNo,
                                        FormId = ggg.FormId
                                    });
                                }
                            }
                        }
                    }
                }
            }

            //List<WebMenuModel> menus = webMenu.OrderBy(ord => ord.ItemId).ToList();
            //_menuString = "";
            //BuildAngularMenu(menus);
            //NewAngularMenu(menus);

            //_menuString = "{\"displayName\":\"A\",\"children\":{\"displayName\":\"AA1 - Page1\"},\"displayName\":\"B\",\"children\":{\"displayName\":\"BB1\",\"children\":{\"displayName\":\"BBB1 - Page3\"}}}";
            //List<NavItem> navMenu = JsonConvert.DeserializeObject<List<NavItem>>(_menuString);

            //return _menuString;
            //return Ok(JsonConvert.SerializeObject(_menuString));
            return webMenu;
            //return navMenu;
        }



        //public List<NavItem> navMenu = new List<NavItem>();


        public void NewAngularMenu(List<WebMenuModel> fullMenu)
        {
            //_menuString = "[";

            foreach (var menu in fullMenu)
            {
                if (menu.ParentId == 0)
                {
                    NewAngularSubMenu(fullMenu, menu);
                }
            }

            //_menuString = _menuString + "];";
        }


        public void NewAngularSubMenu(List<WebMenuModel> fullMenu, WebMenuModel menu)
        {
            //_menuString = _menuString + "{ displayName: '" + menu.DisplayName + "'";
            //navMenu.Add(new NavItem { displayName = menu.DisplayName });

            var subMenus = fullMenu.Where(p => p.ParentId == menu.ItemId);

            if (subMenus.Count() > 0)
            {
                //_menuString = _menuString + ", children: [";

                foreach (WebMenuModel p in subMenus)
                {
                    if (fullMenu.Count(x => x.ParentId == p.ItemId) > 0)
                    {
                        AngularSubMenu(fullMenu, p);
                    }
                    else
                    {
                        //_menuString = _menuString + "{ displayName: '" + p.DisplayName + "'}";
                        //navMenu.Append<NavItem>(new NavItem { displayName = menu.DisplayName });
                    }
                }
                //_menuString = _menuString + " ]";
            }
            //_menuString = _menuString + "}, ";
        }



        //[Route("api/getangularmenu")]
        public void BuildAngularMenu(List<WebMenuModel> fullMenu)
        {
            //_menuString = "[";

            foreach (var menu in fullMenu)
            {
                if (menu.ParentId == 0)
                {
                    AngularSubMenu(fullMenu, menu);
                }
            }

            //_menuString = _menuString + "];";
        }

        
        public void AngularSubMenu(List<WebMenuModel> fullMenu, WebMenuModel menu)
        {
            _menuString = _menuString + "{ displayName: '" + menu.DisplayName + "'";

            var subMenus = fullMenu.Where(p => p.ParentId == menu.ItemId);

            if (subMenus.Count() > 0)
            {
                _menuString = _menuString + ", children: [";

                foreach (WebMenuModel p in subMenus)
                {
                    if (fullMenu.Count(x => x.ParentId == p.ItemId) > 0)
                    { 
                        AngularSubMenu(fullMenu, p);
                    }
                    else
                    {
                        _menuString = _menuString + "{ displayName: '" + p.DisplayName + "'}";
                    }
                }
                _menuString = _menuString + " ]";
            }
            _menuString = _menuString + "}, ";
        }


        public void BuildMenu(List<WebMenuModel> fullMenu)
        {
            _menuString = "<ul class='nav navbar-nav'>";

            foreach (var menu in fullMenu)
            {
                if (menu.ParentId == 0)
                {
                    SubMenu(fullMenu, menu);
                }
            }
            _menuString = _menuString + "</ul>";
        }

        public void SubMenu(List<WebMenuModel> fullMenu, WebMenuModel menu)
        {
            //_menuList = _menuList + "<li [routerLinkActive]=\"['link-active']\">< a[routerLink] = \"['/home']\" >< span class='glyphicon glyphicon-home'></span>" + page.DisplayName + "</a>";
            _menuString = _menuString + "<li><a><span class='glyphicon glyphicon-home'></span>" + menu.DisplayName + "</a>";

            var subMenus = fullMenu.Where(p => p.ParentId == menu.ItemId);

            if (subMenus.Count() > 0)
            {
                _menuString = _menuString + "<ul class='nav navbar-nav'>";

                foreach (WebMenuModel p in subMenus)
                {
                    if (fullMenu.Count(x => x.ParentId == p.ItemId) > 0)
                    {
                        SubMenu(fullMenu, p);
                    }
                    else
                    {
                        //_menuList = _menuList + "<li [routerLinkActive]=\"['link-active']\">< a[routerLink] = \"['/home']\" >< span class='glyphicon glyphicon-th-list'></span>" + p.DisplayName + "</a></li>";
                        _menuString = _menuString + "<li><a><span class='glyphicon glyphicon-th-list'></span>" + p.DisplayName + "</a></li>";
                    }
                }
                _menuString = _menuString + "</ul>";
            }
            _menuString = _menuString + "</li>";
        }
    }
}