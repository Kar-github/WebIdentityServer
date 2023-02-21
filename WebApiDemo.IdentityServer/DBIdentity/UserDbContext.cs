using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Common.Models;
using IdentityServer.DBIdentity;
using IdentityServer.Application.Models;

namespace IdentityServer
{
    //[DbConfigurationType(typeof(DbConfig))]
    public  class UserDbContext:IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        //public static UserDbContext Create() => new UserDbContext();

    }
}
