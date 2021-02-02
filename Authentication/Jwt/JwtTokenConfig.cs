using System;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Jwt
{
    public class JwtTokenConfig
    {
        [Required] public string SecurityKey { get; set; }
        [Required] public string IssuerDomain { get; set; }
        [Required] public string AudienceDomain { get; set; }
        [Range(1, int.MaxValue)] public int ExpiryMinutes { get; set; }
       
    }
}
