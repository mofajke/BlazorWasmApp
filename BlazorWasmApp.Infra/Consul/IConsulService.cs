using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmApp.Infra.Consul
{
    public interface IConsulService
    {
        Task<string> GetByNameAsync(string name);

        Task SaveOrUpdateByNameAsync(string name, string value);

        Task ImportKvPairsAsync(Dictionary<string,string> dict);

        Task<Dictionary<string, string>> GetAllAsync();
    }
}
