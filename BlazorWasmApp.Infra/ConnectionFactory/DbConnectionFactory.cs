using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.DI;
using BlazorWasmApp.Infra.SettingsStore;
using Npgsql;

namespace BlazorWasmApp.Infra.ConnectionFactory
{
    [InjectAsSingleton(typeof(IDbConnectionFactory))]
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private ISettingsStore settingsStore;

        public DbConnectionFactory(ISettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connectionString = await settingsStore.GetAsync("PostgresConnectionString");
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
