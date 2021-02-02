using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Responses.LoginResponses
{
    public class BearerToken
    {
        public BearerToken(
            string accessToken,
            int expiresInSeconds)
        {
            AccessToken = accessToken;
            ExpiresInSeconds = expiresInSeconds;
        }

        public string AccessToken { get; }
        public int ExpiresInSeconds { get; }
    }
}
