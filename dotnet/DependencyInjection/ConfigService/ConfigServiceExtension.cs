using ConfigService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigServiceExtension
    {
        public static void AddConfigService(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IConfigService, EnvironmentConfigService>();
            serviceCollection.AddScoped<IConfigService, FileConfigService>();
        }
    }
}
