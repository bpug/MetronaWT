//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EmailService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Hier den E-Mail-Dienst einfügen, um eine E-Mail-Nachricht zu senden.
            return Task.FromResult(0);
        }
    }
}