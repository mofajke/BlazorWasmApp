using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWasmApp.Infra.SqlLoader
{
    public interface ISqlScriptLoader
    {
        Task<string> GetSqlAsync(string sqlScriptFileName);
    }
}
