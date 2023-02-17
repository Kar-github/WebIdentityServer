using FluentValidation;
using WebApiDemo.Application.Commands;

namespace WebApiDemo.Application.Validations.Commands
{
    public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Code).NotEmpty();
            RuleFor(model => model.Price).GreaterThanOrEqualTo(0);
        }
    }
}
