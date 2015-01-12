//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IdentityExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Extensions
{
    using System.Security.Claims;
    using System.Security.Principal;

    using Metrona.Wt.Identity.Models;

    using ServiceStack;

    public static class IdentityExtensions
    {
        public static ApplicationUser ToAppUser(this IPrincipal source)
        {
            var claims = source as ClaimsPrincipal;
            if (claims == null) return null;
            
            var claim = claims.FindFirst(ClaimTypes.UserData);
            return claim == null ? null : claim.Value.FromJson<ApplicationUser>();
        }
    }
}