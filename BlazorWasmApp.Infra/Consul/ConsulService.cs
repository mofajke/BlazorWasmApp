using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.DI;
using Consul;

namespace BlazorWasmApp.Infra.Consul
{
    [InjectAsSingleton(typeof(IConsulService))]
    public class ConsulService : IConsulService
    {
        private const string prefix = "";
        private const string defaultDevUri = "http://localhost:8500";
        private const string defaultDockerUri = "http://dev-consul:8500"; // {protocol}://{containername}:{port}
        private readonly ConsulClient client;

        public ConsulService()
        {
            client = GetClientAsync(defaultDockerUri).Result ?? GetClientAsync(defaultDevUri).Result;
        }

        public async Task<ConsulClient> GetClientAsync(string uri)
        {
            try
            {
                var checkedClient = new ConsulClient((config) =>
                {
                    config.Address = new Uri(uri);
                    config.WaitTime = TimeSpan.FromSeconds(10);
                });

                var result = await checkedClient.Agent.GetNodeName();
                return string.IsNullOrEmpty(result) ? null : checkedClient;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetByNameAsync(string name)
        {
            var result = await client.KV.Get(name);
            return Encoding.UTF8.GetString(result.Response.Value);
        }

        public async Task SaveOrUpdateByNameAsync(string name, string value)
        {
            var pair = new KVPair(name)
            {
                Value = Encoding.UTF8.GetBytes(value)
            };

            await client.KV.Put(pair);
        }

        public async Task ImportKvPairsAsync(Dictionary<string, string> dict)
        {
            await ClearKvAsync();

            Parallel.ForEach(dict, async (dictPair) =>
            {
                var pair = new KVPair(dictPair.Key)
                {
                    Value = Encoding.UTF8.GetBytes(dictPair.Value)
                };

                await client.KV.Put(pair);
            });
        }

        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            var pairs = await client.KV.List(prefix);
            return pairs.Response.ToDictionary(pair => pair.Key, pair => Encoding.UTF8.GetString(pair.Value));
        }

        private async Task ClearKvAsync()
        {
            var keys = await client.KV.Keys(prefix);

            Parallel.ForEach(keys.Response, async (key) =>
            {
                await client.KV.Delete(key);
            });
        }
    }
}
