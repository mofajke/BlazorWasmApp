using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace BlazorWasmApp.Infra.ConnectionFactory
{
    public interface IRedisDbConnectionFactory
    {
        Task<IConnectionMultiplexer> GetConnectionAsync();
    }
}
