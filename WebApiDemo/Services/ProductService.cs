using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Application.Entity;
using WebApiDemo.Infrastructure.Repasitories;

namespace WebApiDemo.Services
{
    public class ProductService : IProductService
    {
        IProductRepasitory _rep;
        public ProductService(IProductRepasitory repasitory)
        {
            _rep= repasitory;
        }
        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return (_rep.GetNoTrackingAsync());
        }
        public Task CreateProduct(Product product)
        {
            _rep.Add(product);
            return _rep.SaveChangesAsync();
        }
        public Task DeleteProduct(int Id)
        {
            _rep.Remove(_rep.GetNoTrackingAsync(x => x.Id == Id).Result.First());
            return _rep.SaveChangesAsync();
        }
    }
}
