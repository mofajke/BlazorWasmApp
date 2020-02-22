using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.DI;

namespace BlazorWasmApp.Infra.Auth
{
    [InjectAsSingleton(typeof(IChallengeCodeHandler))]
    public class ChallengeCodeHandler : IChallengeCodeHandler
    {
        public Task<string> GetAuthCodeAsync(string challenge_code)
        {
            throw new NotImplementedException();
        }
    }
}
