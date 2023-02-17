using WebApiDemo.Common.CustomExtensions;
using WebApiDemo.Infrastructure.Repasitories;
using WebApiDemo.Infrastructure;
using WebApiDemo.Extensions;
using WebApiDemo.DB.ConnectionProvider;
using Autofac.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using WebApiDemo.Common;
using System.Runtime.CompilerServices;
using System.Reflection;
using WebApiDemo.Services.Infrastructure;
using WebApiDemo.Services.Infrastructure.Behaviors;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllers();
        var identityUrl = "https://localhost:44335";
        var audience = "WebApiDemo";
        builder.Services.ConfigureAuthorization(identityUrl, audience);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureCors();
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Services.ConfigConnection();
        builder.Services.AddRepasitories();
        builder.Services.AddServices();
        builder.Host.ConfigureMediatorContainer();
        builder.Services.AddMediatR(typeof(Program).Assembly);

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}