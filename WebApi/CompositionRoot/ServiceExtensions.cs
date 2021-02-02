using Application.ContractResolvers;
using Application.Extensions;
using Application.Factories;
using Application.Factories.Abstractions;
using Application.Repositories;
using Application.Services.Abstractions;
using Application.ErrorHandlers;
using Arch.EntityFrameworkCore.UnitOfWork;
using Authentication.Jwt;
using Authentication.Jwt.Extensions;
using bookstore_api.Middleware;
using Domain.Services;
using Integration.LocalServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository.EntityFramework.Entities;
using Repository.EntityFramework.Repositories;
using System;
using System.Security.Principal;
using System.Text;
using Application.EmailSender;
using Integration.Email;

namespace bookstore_api.CompositionRoot
{
    public static class ServiceExtensions
    {

        public static void ConfigureWebApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddValueObjectFactoryConfig(config.GetSection("StandingData:FactoryConfiguration"));            
            services.AddJwtTokenConfig(config.GetSection("Security:JwtTokenConfiguration"));
            services.AddHttpVerbExclusionConfig(config.GetSection("Security:VerbExclusions"));        
        }

        public static void AddWebApi(this IServiceCollection services, IConfiguration config)
        {
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Security:JwtTokenConfiguration:IssuerDomain"],
                    ValidAudience = config["Security:JwtTokenConfiguration:AudienceDomain"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(config["Security:JwtTokenConfiguration:SecurityKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSingleton<IDateTimeService, LocalDateTimeService>();
            services.AddSingleton<IObjectTypeService, ObjectTypeService>();

            services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddScoped<ICreateBearerTokenService, JwtTokenService>();
            services.AddScoped<IRefreshBearerTokenService, JwtTokenService>(); 
            services.AddScoped<ValueObjectInternalSetResolver>();
            services.AddScoped<IValueObjectFactory, ValueObjectFactory>();
            services.AddScoped<Func<IValueObjectFactory>>(ctx => () => ctx.GetService<IValueObjectFactory>());

            //SQL Server databases
            services.AddDbContext<DepContext>(opt => opt.UseSqlServer(config.GetConnectionString("BookstoreDataDatabase"), sqlOpt =>
            {
                sqlOpt.UseNetTopologySuite();
            })).AddUnitOfWork<DepContext>();


            services.AddScoped<IProductDataRepository, ProductDataRepository>();
            services.AddScoped<ISubscriptionDataRepository, SubscriptionDataRepository>();
            services.AddScoped<IRepositoryExceptionHandler, RepositoryExceptionHandler>();
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
