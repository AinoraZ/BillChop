using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BillChopBE.Extensions
{
    public static class ServiceCollectionOptionExtensions
    {
        public static OptionsBuilder<T> ConfigureWithValidation<T>(this IServiceCollection serviceCollection, IConfigurationSection section) where T : class
        {
            return serviceCollection.AddOptions<T>()
                .Bind(section)
                .ValidateDataAnnotations();
        }
    }
}
