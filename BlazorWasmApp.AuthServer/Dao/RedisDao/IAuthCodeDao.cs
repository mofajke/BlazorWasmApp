using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWasmApp.Shared.Auth.models;

namespace BlazorWasmApp.AuthServer.Dao.RedisDao
{
    public interface IAuthCodeDao
    {
        Task SaveCodeChallengeAsync(string authCode, string code_challenge);
        Task DeleteByAuthCodeAsync(string authCode);
        Task<string> GetCodeChallengeByAuthCodeAsync(string authCode);
        Task SaveTokenAsync(TokenResponse tokenResponse, long userId);
        Task DeleteByTokenAsync(string token);
    }
}
