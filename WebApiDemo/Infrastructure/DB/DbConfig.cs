using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.DB.ConnectionProvider;

namespace WebApiDemo.Infrastructure
{
    public class DbConfig:DbConfiguration
    {
        public DbConfig()
        {
            SetContextFactory(() =>new DbContextFactory().Create());
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
