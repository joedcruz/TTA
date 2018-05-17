using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTAServer
{
    public class UserManagementDBOps
    {
        protected ApplicationDbContext mContext;

        public UserManagementDBOps(ApplicationDbContext context)
        {
            mContext = context;
        }

        public string GetUserRoles()
        {
            //private ApplicationDbContext context = new ApplicationDbContext;

            mContext.Database.EnsureCreated();
            string xyz = "sss";

            if (mContext.Roles != null)
            {
                var tableRecords = mContext.Roles;

                foreach (var x in tableRecords)
                {
                    var a = x.Name;
                }
            }

            return xyz;
        }
    }
}
