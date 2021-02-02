using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_api.Middleware
{
    public class HttpVerbExclusion
    {
        private readonly RequestDelegate _next;
        private readonly HttpVerbExclusionOptions _config;

        public HttpVerbExclusion(
            RequestDelegate next,
            IOptions<HttpVerbExclusionOptions> config)
        {
            _next = next;
            _config = config.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_config.VerbsToExclude.Any(a => a.Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync("Method Not Allowed");
            }
            await _next.Invoke(context);
        }
    }

    public class HttpVerbExclusionOptions
    {
        public List<string> VerbsToExclude { get; set; }
    }

    internal static class HttpVerbExclusionExtensions
    {
        public static IApplicationBuilder UseHttpVerbExclusions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpVerbExclusion>();
        }

        public static void AddHttpVerbExclusionConfig(this IServiceCollection services, IConfigurationSection config)
        {
            services.AddOptions<HttpVerbExclusionOptions>().Bind(config);
        }
    }
}
