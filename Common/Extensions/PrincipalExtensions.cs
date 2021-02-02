using Common.Guards;
using System.Security.Claims;
using System.Security.Principal;

namespace Common.Extensions
{
    public static class PrincipalExtensions
    {
        public static string Username(this IPrincipal currentPrincipal)
        {
            Ensure.Is<ClaimsPrincipal>(currentPrincipal, nameof(currentPrincipal));
            var claimsPrincipal = currentPrincipal as ClaimsPrincipal;
            return claimsPrincipal.FindFirst(ClaimTypes.Name).Value;
        }
    }
}
