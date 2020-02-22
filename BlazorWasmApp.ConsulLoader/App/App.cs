using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlazorWasmApp.ConsulLoader.Models;
using BlazorWasmApp.Infra.Consul;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorWasmApp.ConsulLoader.App
{
    public class App : IApp
    {
        private readonly IConsulService consulService;
        private readonly ILogger<App> logger;

        public App(IConsulService consulService, ILogger<App> logger)
        {
            this.consulService = consulService;
            this.logger = logger;
        }

        public async Task ExecuteAsync(string env)
        {
            var dict = await LoadJsonAsync(env);
            logger.LogDebug("Json cfg loaded");

            await consulService.ImportKvPairsAsync(dict);
            logger.LogDebug("Settings imported to Consul");
        }

        private async Task<Dictionary<string, string>> LoadJsonAsync(string env)
        {
            var assembly = typeof(BlazorWasmApp.ConsulLoader.Program).GetTypeInfo().Assembly;

            await using var resource = assembly.GetManifestResourceStream($"BlazorWasmApp.ConsulLoader.{env}.json");

            if (resource == null)
                return null;

            using var streamReader = new StreamReader(resource);
            var jsonString = await streamReader.ReadToEndAsync();
            var json = JsonConvert.DeserializeObject<List<RootObject>>(jsonString);
            return json.ToDictionary(r => r.Key, r => json.FirstOrDefault(val => val.Key == r.Key)?.Value);
        }
    }
}