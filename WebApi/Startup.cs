using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using bookstore_api.CompositionRoot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using MediatR;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using bookstore_api.Middleware;
using System.Threading;
using bookstore_api.Filters;
using FluentValidation.AspNetCore;
using Domain.Rules.Validation;
using Application.Requests.ProductDataRequests;

namespace bookstore_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMvc(opt =>
            {
                ////****For now I am disabling the JWT auth, but in production this will be put back
                var authorizationPolicy =
                    new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                opt.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                ////****

                opt.EnableEndpointRouting = false;
                opt.Filters.Add<ActionLogFilter>();
            })
          .SetCompatibilityVersion(CompatibilityVersion.Latest)
          .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<ProductDataValidator>());

            services.AddCors();
            services.ConfigureWebApi(Configuration);
            services.AddWebApi(Configuration);

            Assembly[] myAssemblies = Thread.GetDomain().GetAssemblies();
            Assembly myAssembly = null;
            
            string versionnr = "";

            for (int i = 0; i < myAssemblies.Length; i++)
            {
                if (String.Compare(myAssemblies[i].GetName().Name, "bookstore_api") == 0)
                    myAssembly = myAssemblies[i];
            }
            if (myAssembly != null)
            {
                versionnr = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore REST API", Version = versionnr });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });

                s.OperationFilter<BearerTokenOperationsFilter>();

            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(typeof(ListProductData).Assembly);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseGlobalExceptionHandler();
            }

            app.UseCors(opt =>
            {
                opt
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .WithMethods(new[] { "GET", "POST", "DELETE", "PATCH" });
            });

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("v1/swagger.json", "Bookstore REST API v1");
            });

            app.UseMvc();
            app.UseHttpVerbExclusions();
        }
    }
}
