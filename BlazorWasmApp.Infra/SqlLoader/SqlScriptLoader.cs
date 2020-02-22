using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.DI;

namespace BlazorWasmApp.Infra.SqlLoader
{
    [InjectAsSingleton(typeof(ISqlScriptLoader))]
    public class SqlScriptLoader : ISqlScriptLoader
    {
        private Dictionary<string, string> dictScripts;

        public async Task<string> GetSqlAsync(string sqlScriptFileName)
        {
            if (dictScripts == null)
            {
                dictScripts = await GetAllAsync();
            }

            return dictScripts.ContainsKey(sqlScriptFileName) ? dictScripts[sqlScriptFileName] : null;
        }

        private async Task<Dictionary<string, string>> GetAllAsync()
        {
            var result = new Dictionary<string,string>();
            var assemblies = DIResolver.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var resourceNames = assembly.GetManifestResourceNames();
                foreach (var resourceName in resourceNames)
                {
                    var resourceStream = assembly.GetManifestResourceStream(resourceName);
                    if (resourceStream == null)
                    {
                        continue;
                    }

                    using (var reader = new System.IO.StreamReader(resourceStream, Encoding.UTF8))
                    {
                        var sql = await reader.ReadToEndAsync();
                        result.Add(resourceName, sql);
                    }
                }
            }
            return result;
        }
    }
}
