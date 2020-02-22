using BlazorWasmApp.Infra.Consul;
using BlazorWasmApp.Infra.DI;
using BlazorWasmApp.Infra.SettingsStore;
using BlazorWasmApp.Infra.SqlLoader;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWasmApp.Infra.Extensions.ServiceCollection
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfra(this IServiceCollection collection)
        {
            DIResolver.Resolve(collection);
        }
    }
}
