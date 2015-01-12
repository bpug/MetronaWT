//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UserExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity
{
    using Metrona.Wt.Database.Models;
    using Metrona.Wt.Identity.Models;

    public static class UserExtensions
    {
        public static ApplicationUser ToApplicationUser(this User source)
        {
            if (source == null)
            {
                return null;
            }
            var result = new ApplicationUser
            {
                UserName = source.Username,
                Password = source.Password
            };

            return result;
        }
    }
}