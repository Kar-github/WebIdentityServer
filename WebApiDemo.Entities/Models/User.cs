using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Entities.Models
{
    public  class User: IdentityUser
    {
        public User()
        {

        }
        public override string UserName { get; set; }   
        public string Password { get; set; }
    }
}
