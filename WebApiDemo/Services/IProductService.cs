using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Application.Entity;

namespace WebApiDemo.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task CreateProduct(Product product);
        Task DeleteProduct(int Id);
    }
}
