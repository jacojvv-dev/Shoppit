using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var subjectClaim =  principal.Claims.FirstOrDefault(claim =>  claim.Type == OpenIddictConstants.Claims.Subject);
            return subjectClaim == null ? default : Guid.Parse(subjectClaim.Value);
        }
    }
}