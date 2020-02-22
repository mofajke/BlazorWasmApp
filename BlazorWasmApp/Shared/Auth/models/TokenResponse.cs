using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmApp.Shared.Auth.models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpire { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
    }
}
