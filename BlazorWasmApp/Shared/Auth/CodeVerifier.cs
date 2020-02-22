using System;

namespace BlazorWasmApp.Shared.Auth
{
    public static class CodeVerifier
    {
        private const int codeVerifierMinLength = 43;
        private const int codeVerifierMaxLength = 128;

        private static readonly char[] availableChars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'X', 'Y', 'Z',
                                                          'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'v', 'x', 'y', 'z',
                                                          '-', '.', '_', '~' };

        public static string Create()
        {
            var codeVerifierLength = GetCodeVerifierLength();
            var codeVerifier = string.Empty;
            for (var i = 0; i <= codeVerifierLength; i++)
            {
                codeVerifier += GetRandomSymbol();
            }

            return codeVerifier;
        }

        private static int GetRandomSymbol()
        {
            return GetRandomNumber(0, availableChars.Length - 1);
        }

        private static int GetCodeVerifierLength()
        {
            return GetRandomNumber(codeVerifierMinLength, codeVerifierMaxLength) - 1;
        }

        private static int GetRandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }
    }
}
