using WebApiDemo.Common.Models;

namespace WebApiDemo.Application.Entity
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
    }
}
