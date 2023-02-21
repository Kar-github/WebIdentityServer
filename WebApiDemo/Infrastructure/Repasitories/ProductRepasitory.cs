using WebApiDemo.Application.Entity;
using WebApiDemo.Infrastructure.DB;
using WebApiDemo.Services.Infrastructure;

namespace WebApiDemo.Infrastructure.Repasitories
{
    public class ProductRepasitory :Repository<Product>, IProductRepasitory
    {
        public ProductRepasitory(WebApiDemoDbContext db):base(db)
        {
        }

    }
}
