using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrona.Wt.Identity
{
    using System.Security.Claims;

    using Metrona.Wt.Identity.Models;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    // Anwendungsanmelde-Manager konfigurieren, der in dieser Anwendung verwendet wird.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, long>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            //return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
            return ((ApplicationUserManager)UserManager).GenerateUserIdentityAsync(user);
        }

        //public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        //{
        //    return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        //}

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            SignInStatus signInStatus;
            if (UserManager == null)
            {
                signInStatus = SignInStatus.Failure;
            }
            else
            {
                var user = await this.UserManager.FindByNameAsync(userName);
                if (user == null)
                    signInStatus = SignInStatus.Failure;

                else if (await (UserManager.CheckPasswordAsync(user, password)))
                {
                    await SignInAsync(user, isPersistent, false);
                    signInStatus = SignInStatus.Success;
                }
                else
                {
                    signInStatus = SignInStatus.Failure;
                }
            }

            return signInStatus;
        }
    }
}
