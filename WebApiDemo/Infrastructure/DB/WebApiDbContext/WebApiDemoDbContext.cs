using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Application.Entity;

namespace WebApiDemo.Infrastructure.DB
{
    [DbConfigurationType(typeof(DbConfig))]
    public partial class WebApiDemoDbContext : DbContext
    {
        public WebApiDemoDbContext(string connectionstring) : base(connectionstring)
        {
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureProduct(modelBuilder.Entity<Product>());

        }
    }
}
