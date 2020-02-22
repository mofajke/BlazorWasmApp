using System.Threading.Tasks;
using BlazorWasmApp.AuthServer.Services;
using BlazorWasmApp.Shared.Auth;
using BlazorWasmApp.Shared.Auth.models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmApp.AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthCodeService authCodeService;

        public AuthorizationController(IAuthCodeService authCodeService)
        {
            this.authCodeService = authCodeService;
        }

        [HttpPost]
        [Route("AuthCode")]
        public async Task<IActionResult> GetAuthCode(CodeChallengeRequest request)
        {
            if (!string.IsNullOrEmpty(request?.CodeChallenge))
            {
                var code = await authCodeService.GenerateAuthCodeAndSaveAsync(request.CodeChallenge);
                return new JsonResult(new CodeAnswer {Code = code});
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Token")]
        public async Task<IActionResult> GetToken(TokenRequest request)
        {
            if (IsTokenRequestValid(request))
            {
                return new JsonResult(await authCodeService.GenerateTokenAsync(request));
            }

            return UnprocessableEntity();
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> GetTokenByRefresh(RefreshTokenRequest request)
        {
            if (IsRefreshTokenRequestValid(request))
            {
                return new JsonResult(await authCodeService.GenerateTokenByRefreshTokenAsync(request));
            }

            return UnprocessableEntity();
        }

        private static bool IsTokenRequestValid(TokenRequest request)
        {
            return request != null
                   && !string.IsNullOrEmpty(request.AuthCode)
                   && !string.IsNullOrEmpty(request.CodeVerifier)
                   && !string.IsNullOrEmpty(request.Login)
                   && !string.IsNullOrEmpty(request.Password);
        }

        private static bool IsRefreshTokenRequestValid(RefreshTokenRequest request)
        {
            return request != null
                   && !string.IsNullOrEmpty(request.AuthCode)
                   && !string.IsNullOrEmpty(request.CodeVerifier)
                   && !string.IsNullOrEmpty(request.RefreshToken)
                   && !string.IsNullOrEmpty(request.ExpiredToken);
        }
    }
}