using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Jwt.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwtTokenConfig(this IServiceCollection services, IConfigurationSection config)
        {
            services.AddOptions<JwtTokenConfig>().Bind(config).ValidateDataAnnotations();
        }
    }
}
