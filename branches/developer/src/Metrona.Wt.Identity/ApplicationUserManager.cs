//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ApplicationUserManager.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Metrona.Wt.Identity.Models;
    using Metrona.Wt.Identity.Services;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using Newtonsoft.Json;

    using ServiceStack;

    // Konfigurieren des in dieser Anwendung verwendeten Anwendungsbenutzer-Managers. UserManager wird in ASP.NET Identity definiert und von der Anwendung verwendet.
    public class ApplicationUserManager : UserManager<ApplicationUser, long>
    {
        private readonly IPasswordHasher passwordHasher;
        public ApplicationUserManager(IUserStore<ApplicationUser, long> store, IPasswordHasher passwordHasher)
            : base(store)
        {
            this.passwordHasher = passwordHasher;
            this.Configure();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser user)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await this.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim(ClaimTypes.UserData, user.ToJson()));
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }

        private void Configure()
        {
            //var manager =new ApplicationUserManager(ServiceLocator.Current.GetInstance<IUserStoreService>());

            // Konfigurieren der Überprüfungslogik für Benutzernamen.
            this.PasswordHasher = this.passwordHasher;

            this.UserValidator = new UserValidator<ApplicationUser, long>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Konfigurieren der Überprüfungslogik für Kennwörter.
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Standardeinstellungen für Benutzersperren konfigurieren
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registrieren von Anbietern für zweistufige Authentifizierung. Diese Anwendung verwendet telefonische und E-Mail-Nachrichten zum Empfangen eines Codes zum Überprüfen des Benutzers.
            // Sie können Ihren eigenen Anbieter erstellen und hier einfügen.
            this.RegisterTwoFactorProvider(
                "Telefoncode",
                new PhoneNumberTokenProvider<ApplicationUser, long>
                {
                    MessageFormat = "Ihr Sicherheitscode lautet {0}"
                });
            this.RegisterTwoFactorProvider(
                "E-Mail-Code",
                new EmailTokenProvider<ApplicationUser, long>
                {
                    Subject = "Sicherheitscode",
                    BodyFormat = "Ihr Sicherheitscode lautet {0}"
                });
            this.EmailService = new EmailService();
            this.SmsService = new SmsService();

            var dataProtectionProvider = IdentityStartup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser, long>(
                        dataProtectionProvider.Create("Brunata Klima Identity"));
            }
        }
    }
}