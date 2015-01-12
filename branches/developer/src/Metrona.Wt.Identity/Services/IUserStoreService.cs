//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IUserStoreService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity.Services
{
    using Metrona.Wt.Identity.Models;

    using Microsoft.AspNet.Identity;

    public interface IUserStoreService : IUserPasswordStore<ApplicationUser, long>
    {
    }
}