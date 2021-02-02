using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Application.Responses.LoginResponses
{
    public class PostLoginResponse
    {
        [Required]
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType => "bearer";

        [Required]
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [Required]
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresInSeconds { get; set; }
    }
}
