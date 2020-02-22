using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmApp.Shared.Auth.models
{
    public class TokenRequest
    {
        public string AuthCode { get; set; }

        public string CodeVerifier { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
