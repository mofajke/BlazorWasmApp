using System.Threading.Tasks;
using BlazorWasmApp.ConsulLoader.App;
using BlazorWasmApp.Infra.Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace BlazorWasmApp.ConsulLoader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IApp, App.App>()
                .AddSingleton<IConsulService, ConsulService>()
                .BuildServiceProvider();

            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            var app = serviceProvider.GetService<IApp>();
            await app.ExecuteAsync(args.Length == 1 ? args[0] : "dev");
        }
    }
}
