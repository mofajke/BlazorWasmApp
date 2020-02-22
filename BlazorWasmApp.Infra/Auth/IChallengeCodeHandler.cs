using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWasmApp.Infra.Auth
{
    public interface IChallengeCodeHandler
    {
        Task<string> GetAuthCodeAsync(string challenge_code);
    }
}
