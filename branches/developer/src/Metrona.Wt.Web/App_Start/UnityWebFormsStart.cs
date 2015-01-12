using System.Web;

using Microsoft.Practices.Unity;
using Unity.WebForms;

[assembly: WebActivator.PostApplicationStartMethod( typeof(Metrona.Wt.Web.App_Start.UnityWebFormsStart), "PostStart" )]
namespace Metrona.Wt.Web.App_Start
{
    using Metrona.Wt.Database;
    using Metrona.Wt.Database.Models;
    using Metrona.Wt.Database.Repositories;
    using Metrona.Wt.Identity;
    using Metrona.Wt.Identity.Models;
    using Metrona.Wt.Identity.Services;
    using Metrona.Wt.Service;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
	///		Startup class for the Unity.WebForms NuGet package.
	/// </summary>
	internal static class UnityWebFormsStart
	{
		/// <summary>
		///     Initializes the unity container when the application starts up.
		/// </summary>
		/// <remarks>
		///		Do not edit this method. Perform any modifications in the
		///		<see cref="RegisterDependencies" /> method.
		/// </remarks>
		internal static void PostStart()
		{
			IUnityContainer container = new UnityContainer();
			HttpContext.Current.Application.SetContainer( container );

			RegisterDependencies( container );

            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
		}

		/// <summary>
		///		Registers dependencies in the supplied container.
		/// </summary>
		/// <param name="container">Instance of the container to populate.</param>
		private static void RegisterDependencies( IUnityContainer container )
		{
            var configuration = new Configuration();

            container.RegisterType<IEntitiesContext, KlimaContext>(
                new PerResolveLifetimeManager(),
                new InjectionConstructor(configuration.ConnectionString));

            container.RegisterType<IUserStore<ApplicationUser, long>, UserStoreService>();
            container.RegisterType<UserManager<ApplicationUser, long>, ApplicationUserManager>();
            container.RegisterType<SignInManager<ApplicationUser, long>, ApplicationSignInManager>();

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IWetterStationRepository, WetterStationRepository>();
            container.RegisterType<IBundeslandRepository, BundeslandRepository>();
            container.RegisterType<IKlimaRepository, KlimaRepository>();
            container.RegisterType<IMeteoGtzRepository, MeteoGtzRepository>();
            container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterType<IBundeslandService, BundeslandService>();
            container.RegisterType<IKlimaService, KlimaService>();
            container.RegisterType<IMeteoGtzService, MeteoGtzService>();
            //container.RegisterType<IUserStoreService, UserStoreService>();

            //container.RegisterType<WitterungstelegramCreateReport>( new InjectionProperty("BundeslandService"));
		}
	}
}