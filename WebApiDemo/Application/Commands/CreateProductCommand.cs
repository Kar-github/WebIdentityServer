using MediatR;
using WebApiDemo.Application.Entity;
using WebApiDemo.Infrastructure.Repasitories;
using WebApiDemo.Services;
using WebApiDemo.Services.Infrastructure.Requests;

namespace WebApiDemo.Application.Commands
{
    public class CreateProductCommand:BaseRequest<bool>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
    }
    public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand,bool>
    {
        private readonly IProductService _service;
        public CreateProductCommandHandler(IProductService service)
        {
            _service = service;
        }
            
        public async Task<bool> Handle(CreateProductCommand request,CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = request.Name,
                Code = request.Code,
                Price = request.Price
            };
            await _service.CreateProduct(product);
            return true;
        }
    }
}
