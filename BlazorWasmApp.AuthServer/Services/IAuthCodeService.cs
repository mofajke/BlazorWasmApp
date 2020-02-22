using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWasmApp.Shared.Auth.models;

namespace BlazorWasmApp.AuthServer.Services
{
    public interface IAuthCodeService
    {
        Task<string> GenerateAuthCodeAndSaveAsync(string code_challenge);
        Task<TokenResponse> GenerateTokenAsync(TokenRequest request);
        Task<TokenResponse> GenerateTokenByRefreshTokenAsync(RefreshTokenRequest request);
    }
}
