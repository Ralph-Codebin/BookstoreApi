using Application.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ValueObjectFactoryExtensions
    {
        public static void AddValueObjectFactoryConfig(this IServiceCollection services, IConfigurationSection config)
        {
            services.AddOptions<ValueObjectFactoryConfig>().Bind(config).ValidateDataAnnotations();
        }
    }
}
