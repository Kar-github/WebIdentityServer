using Autofac;
using FluentValidation;
using MediatR;
using System.Reflection;
using WebApiDemo.Services.Infrastructure.Behaviors;

namespace WebApiDemo.Services.Infrastructure
{
    public class MediatorModule:Autofac.Module
    {
        readonly Assembly[] _assemblies;
        //Assembly[] _customMediatorAssemblies;

        public MediatorModule(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
            foreach (var assembly in _assemblies)
            {
                // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
                builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));

                // Register the Command's Validators (Validators based on FluentValidation library)
                builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClosedTypeOf(typeof(IValidator<>))).AsImplementedInterfaces();
                builder.RegisterAssemblyTypes(assembly)
                    .AsClosedTypesOf(typeof(INotificationHandler<>));
            }
            // Register all the Command classes (they implement IRequestHandler)
            // in assembly holding the Commands

            RegisterPipelines(builder);
        }

        public virtual void RegisterPipelines(ContainerBuilder builder)
        {
            OptionalRegisterPipeline(builder, typeof(ValidatorBehavior<,>));
            OptionalRegisterPipeline(builder, typeof(LoggingBehaviour<,>));
        }
        public void OptionalRegisterPipeline(ContainerBuilder builder, Type type, bool register=true)
        {
            if (register)
            {
                builder.RegisterGeneric(type).As(typeof(IPipelineBehavior<,>));
            }
        }
    }
}
