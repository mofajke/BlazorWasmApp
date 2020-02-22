using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.Consul;
using BlazorWasmApp.Infra.DI;

namespace BlazorWasmApp.Infra.SettingsStore
{
    [InjectAsSingleton(typeof(ISettingsStore))]
    public class SettingsStore : ISettingsStore
    {
        private readonly IConsulService consulService;
        private Dictionary<string, string> store;

        public SettingsStore(IConsulService consulService)
        {
            this.consulService = consulService;
        }

        public async Task<string> GetAsync(string name)
        {
            if (store == null)
            {
                store = await consulService.GetAllAsync();
            }

            return store.ContainsKey(name) ? store[name] : null;
        }

        public async Task LoadAsync()
        {
            store = await consulService.GetAllAsync();
        }
    }
}
