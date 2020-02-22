using System.Threading.Tasks;

namespace BlazorWasmApp.Infra.SettingsStore
{
    public interface ISettingsStore
    {
        Task<string> GetAsync(string name);

        Task LoadAsync();
    }
}
