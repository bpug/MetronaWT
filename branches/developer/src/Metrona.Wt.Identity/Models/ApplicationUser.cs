//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ApplicationUser.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity.Models
{
    using Microsoft.AspNet.Identity;

    public class ApplicationUser : IUser<long>
    {
        public long Id { get; set; }

        public string Password { get; set; }

        //public string PasswordHash { get; set; }

        public string UserName { get; set; }
    }
}