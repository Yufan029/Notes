using LoggerService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoggerServiceExtension
    {
        public static void AddLoggerService(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILoggerService, ConsoleLoggerService>();
        }
    }
}
