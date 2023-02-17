using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApiDemo.DB.ConnectionProvider;
using WebApiDemo.Infrastructure.DB;

namespace WebApiDemo.Infrastructure
{
    public class DbContextFactory: IDbContextFactory<WebApiDemoDbContext>
    {
        public DbContextFactory()
        {
           
        }
        public WebApiDemoDbContext Create()
        {
            var config= new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true).Build();

            return new WebApiDemoDbContext(config.GetConnectionString("DefaultConnectionString"));
        }
    }
}
