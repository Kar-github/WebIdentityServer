using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiDemo.Common.CustomExtensions;
using IdentityServer;
using IdentityServer.DBIdentity;
using IdentityServer.Extensions;
using IdentityServer.Services;
using System.Net;
using IdentityServer4.Services;
using System;
using IdentityServer4.EntityFramework.DbContexts;
using WebApiDemo.Common;
using WebApiDemo.Common.Notification;
using IdentityServer.Application;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
//SeedData.EnsureSeedData(builder.Configuration.GetConnectionString("DefaultConnection"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddDbContext<UserDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity();
builder.Services.AddGoogleAuthentication(builder.Configuration);
builder.Services.ConfigIdentityServer(builder.Configuration);

builder.Services.Configure<EmailOptions>(option => builder.Configuration.GetSection("EmailConfiguration").Bind(option));
builder.Services.Configure<IdentityServerSettings>(option => builder.Configuration.GetSection("IdentityServerSettings").Bind(option));

builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
//app.MigrateDatabase<PersistedGrantDbContext>();
//app.MigrateDatabase<ConfigurationDbContext>();
//app.MigrateDatabase<UserDbContext>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("GlobalPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();
app.Run();
