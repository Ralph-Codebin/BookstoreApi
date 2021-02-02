using Application.Models;
using Application.Services.Abstractions;
using Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Optional;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Application.Responses.LoginResponses;
using Microsoft.Extensions.Configuration;

namespace Authentication.Jwt
{
    public class logedinuser : IApplicationUser
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public IReadOnlyCollection<string> Groups { get; set; }
    }

    public class JwtTokenService : ICreateBearerTokenService, IRefreshBearerTokenService
    {
        private readonly JwtTokenConfig _config;
        private readonly IConfiguration _apiconfig;
        private readonly IDateTimeService _dateTimeService;

        public JwtTokenService(
            IOptions<JwtTokenConfig> config,
            IDateTimeService dateTimeService,
             IConfiguration apiconfig)
        {
            _dateTimeService = dateTimeService;
            _config = config.Value;
            _apiconfig = apiconfig;
        }

        public async Task<Option<BearerToken>> CreateTokenAsync(string username, string password)
        {
            var userName = _apiconfig.GetValue<string>("APILogin:Username");
            var passWord = _apiconfig.GetValue<string>("APILogin:Password");

            if (userName != username || passWord != password)
            {
                return Option.None<BearerToken>();
            }
            else {
                logedinuser user = new logedinuser
                {
                    Username = userName,
                    DisplayName = userName,
                    Groups = new List<string>()
                };
                return CreateToken(user).Some();
            }           
        }

        public Option<BearerToken> RefreshToken(IPrincipal authenticatedUser)
        {
            if(authenticatedUser is ClaimsPrincipal claimsPrincipal)
            {
                return Option.Some(CreateToken(claimsPrincipal.Claims));
            }
            return Option.None<BearerToken>();
        }

        private BearerToken CreateToken(IEnumerable<Claim> claims)
        {
            var now = _dateTimeService.Now;
            var secret = Encoding.UTF8.GetBytes(_config.SecurityKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               _config.IssuerDomain,
               _config.AudienceDomain,
               claims,
               now,
               now.AddMinutes(_config.ExpiryMinutes),
               credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var bearerToken = new BearerToken(tokenHandler.WriteToken(token), _config.ExpiryMinutes * 60);
            return bearerToken;
        }

        private BearerToken CreateToken(IApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };
            claims.AddRange(user.Groups.Select(s => new Claim(ClaimTypes.Role, s)));
            return CreateToken(claims);
        }
    }
}
