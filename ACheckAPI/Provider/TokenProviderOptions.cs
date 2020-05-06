namespace ACheckAPI.Provider
{
    using Microsoft.IdentityModel.Tokens;
    using System;

    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";

        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(+1);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
