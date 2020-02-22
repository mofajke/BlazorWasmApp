using System.Threading.Tasks;

namespace BlazorWasmApp.ConsulLoader.App
{
    public interface IApp
    {
        Task ExecuteAsync(string env);
    }
}
