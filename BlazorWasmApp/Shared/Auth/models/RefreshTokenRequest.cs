using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmApp.Shared.Auth.models
{
    public class RefreshTokenRequest
    {
        public string AuthCode { get; set; }

        public string CodeVerifier { get; set; }

        public string RefreshToken { get; set; }

        public string ExpiredToken { get; set; }
    }
}
