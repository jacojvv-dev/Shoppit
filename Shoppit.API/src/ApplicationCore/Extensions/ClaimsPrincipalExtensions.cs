using System;
using System.Linq;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace ApplicationCore.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Claim GetClaimByType(this ClaimsPrincipal principal, string type)
            => principal.Claims.FirstOrDefault(claim => claim.Type == type);

        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var subjectClaim = principal.GetClaimByType(OpenIddictConstants.Claims.Subject);
            return subjectClaim == null ? default : Guid.Parse(subjectClaim.Value);
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
            => principal.GetClaimByType(OpenIddictConstants.Claims.Name)?.Value;
    }
}