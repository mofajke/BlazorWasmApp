using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlazorWasmApp.Shared.Auth
{
    public static class CodeChallenge
    {
        public static string Create(string codeVerifier)
        {
            return ToBase64Url(ToSha256(ToAscii(codeVerifier)));
        }

        private static byte[] ToAscii(string codeVerifier)
        {
            return Encoding.ASCII.GetBytes(codeVerifier);
        }

        private static byte[] ToSha256(byte[] asciiCodeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(asciiCodeVerifier);
            }
        }

        private static string ToBase64Url(byte[] sha256Bytes)
        {
            return Convert.ToBase64String(sha256Bytes);
        }
    }
}
