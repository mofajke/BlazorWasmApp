using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.DI;
using BlazorWasmApp.Infra.SettingsStore;
using StackExchange.Redis;

namespace BlazorWasmApp.Infra.ConnectionFactory
{
    [InjectAsSingleton(typeof(IRedisDbConnectionFactory))]
    public class RedisDbConnectionFactory : IRedisDbConnectionFactory
    {
        private readonly ISettingsStore settingsStore;
        
        public RedisDbConnectionFactory(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public async Task<IConnectionMultiplexer> GetConnectionAsync()
        {
            var connectionString = await settingsStore.GetAsync("RedisConnectionString");
            return await ConnectionMultiplexer.ConnectAsync(connectionString);
        }
    }
}
