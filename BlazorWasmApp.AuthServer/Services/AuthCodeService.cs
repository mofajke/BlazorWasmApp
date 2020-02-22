using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWasmApp.AuthServer.Dao;
using BlazorWasmApp.AuthServer.Dao.RedisDao;
using BlazorWasmApp.AuthServer.Models;
using BlazorWasmApp.Infra.DI;
using BlazorWasmApp.Shared.Auth;
using BlazorWasmApp.Shared.Auth.models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using BlazorWasmApp.Infra.SettingsStore;
using Microsoft.IdentityModel.Tokens;

namespace BlazorWasmApp.AuthServer.Services
{
    [InjectAsSingleton(typeof(IAuthCodeService))]
    public class AuthCodeService : IAuthCodeService
    {
        private const int tokenLifeTimeInMinutes = 2;
        private const string validIssuer = "tstIssuer";
        private const string validAudience = "tstAudience";
        private const int refreshTokenLifeTimeInDays = 30;

        private readonly IAuthCodeDao dao;
        private readonly IUsersDao usersDao;
        private readonly ISettingsStore settingsStore;

        public AuthCodeService(IAuthCodeDao dao, IUsersDao usersDao, ISettingsStore settingsStore)
        {
            this.dao = dao;
            this.usersDao = usersDao;
            this.settingsStore = settingsStore;
        }

        public async Task<string> GenerateAuthCodeAndSaveAsync(string code_challenge)
        {
            var authCode = GenerateRandomGuid();
            await dao.SaveCodeChallengeAsync(authCode, code_challenge);
            return authCode;
        }

        public async Task<TokenResponse> GenerateTokenAsync(TokenRequest request)
        {
            var isCodeChallengeValid = await IsCodeChallengeValid(request.AuthCode, request.CodeVerifier);
            if (!isCodeChallengeValid) return null;

            var user = await usersDao.GetUserByLoginAndPasswordAsync(request.Login, request.Password);

            var tokenResponse = user != null ? await GenerateTokenAsync(user.Id) : null;

            if (tokenResponse == null) return null;
            await dao.SaveTokenAsync(tokenResponse, user.Id);
            return tokenResponse;
        }

        public async Task<TokenResponse> GenerateTokenByRefreshTokenAsync(RefreshTokenRequest request)
        {
            var isCodeChallengeValid = await IsCodeChallengeValid(request.AuthCode, request.CodeVerifier);
            if (!isCodeChallengeValid) return null;

            await dao.DeleteByTokenAsync(request.ExpiredToken);
            await dao.DeleteByTokenAsync(request.RefreshToken);
            var userId = await GetUserIdFromTokenAsync(request.ExpiredToken);
            var tokenResponse = await GenerateTokenAsync(userId);

            if (tokenResponse == null) return null;
            await dao.SaveTokenAsync(tokenResponse, userId);
            return tokenResponse;
        }

        private async Task<bool> IsCodeChallengeValid(string authCode, string codeVerifier)
        {
            var savedCodeChallenge = await dao.GetCodeChallengeByAuthCodeAsync(authCode);
            var checkedCodeChallenge = CodeChallenge.Create(codeVerifier);
            return savedCodeChallenge == checkedCodeChallenge;
        }

        private async Task<long> GetUserIdFromTokenAsync(string token)
        {
            var cert = await GetCertificateAsync();
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new X509SecurityKey(cert),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = validAudience,
                ValidIssuer = validIssuer
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return Convert.ToInt64(claims.FindFirst("UserId").Value);
        }

        private async Task<TokenResponse> GenerateTokenAsync(long userId)
        {
            var claims = new Claim[] { new Claim("UserId", userId.ToString()) };
            var cert = await GetCertificateAsync();
            var credentials = new SigningCredentials(new X509SecurityKey(cert), SecurityAlgorithms.RsaSha256Signature);
            var tokenExpire = DateTime.Now.AddMinutes(tokenLifeTimeInMinutes);

            var token = new JwtSecurityToken(issuer: validIssuer, audience: validAudience, claims: claims, expires: tokenExpire, signingCredentials: credentials);

            var resultToken = new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenExpire = tokenExpire,
                RefreshToken = GenerateRandomGuid(),
                RefreshTokenExpire = DateTime.Now.AddDays(refreshTokenLifeTimeInDays)
            };

            return resultToken;
        }

        private async Task<X509Certificate2> GetCertificateAsync()
        {
            var certFileName = await settingsStore.GetAsync("CertificateX509FileName");
            var certPassword = await settingsStore.GetAsync("CertificateX509Password");
            var fullPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Certs/{certFileName}";

            return new X509Certificate2(fullPath, certPassword);
        }

        private static string GenerateRandomGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
