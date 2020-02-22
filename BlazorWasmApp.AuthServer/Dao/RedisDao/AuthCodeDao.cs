using System;
using System.Threading.Tasks;
using BlazorWasmApp.Infra.ConnectionFactory;
using BlazorWasmApp.Infra.DI;
using BlazorWasmApp.Shared.Auth.models;
using Newtonsoft.Json;

namespace BlazorWasmApp.AuthServer.Dao.RedisDao
{
    [InjectAsSingleton(typeof(IAuthCodeDao))]
    public class AuthCodeDao : IAuthCodeDao
    {
        private readonly IRedisDbConnectionFactory redisConnectionFactory;

        public AuthCodeDao(IRedisDbConnectionFactory redisConnectionFactory)
        {
            this.redisConnectionFactory = redisConnectionFactory;
        }

        public async Task SaveCodeChallengeAsync(string authCode, string code_challenge)
        {
            using var connection = await redisConnectionFactory.GetConnectionAsync();
            var db = connection.GetDatabase(0);
            await db.StringSetAsync(authCode, code_challenge, TimeSpan.FromMinutes(2));
        }
        
        public async Task DeleteByAuthCodeAsync(string authCode)
        {
            using var connection = await redisConnectionFactory.GetConnectionAsync();
            var db = connection.GetDatabase(0);
            await db.KeyDeleteAsync(authCode);
        }

        public async Task<string> GetCodeChallengeByAuthCodeAsync(string authCode)
        {
            using var connection = await redisConnectionFactory.GetConnectionAsync();
            var db = connection.GetDatabase(0);
            var codeChallenge = await db.StringGetAsync(authCode);
            return codeChallenge.HasValue ? codeChallenge.ToString() : string.Empty;
        }

        public async Task SaveTokenAsync(TokenResponse tokenResponse, long userId)
        {
            var jsonTokenResponse = JsonConvert.SerializeObject(tokenResponse);

            using var connection = await redisConnectionFactory.GetConnectionAsync();
            var db = connection.GetDatabase(0);
            await db.StringSetAsync(userId.ToString(), jsonTokenResponse);
        }

        public async Task DeleteByTokenAsync(string token)
        {
            using var connection = await redisConnectionFactory.GetConnectionAsync();
            var db = connection.GetDatabase(0);
            await db.KeyDeleteAsync(token);
        }
    }
}
