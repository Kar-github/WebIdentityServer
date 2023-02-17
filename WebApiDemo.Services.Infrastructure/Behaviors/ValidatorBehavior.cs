using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Services.Infrastructure.Requests;

namespace WebApiDemo.Services.Infrastructure.Behaviors
{
    public  class ValidatorBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse>
        where TRequest:BaseRequest<TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;
        public ValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request,  RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                    throw new CommandValidationException("Command validation error", new ValidationException(failures));
            }
            return await next();
        }

    }
}
