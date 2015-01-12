using System.Web;

using Microsoft.Practices.Unity;

namespace Metrona.Wt.Web.App_Start
{
    using System;

    using Metrona.Wt.Database;
    using Metrona.Wt.Database.Models;
    using Metrona.Wt.Database.Repositories;
    using Metrona.Wt.Identity;
    using Metrona.Wt.Identity.Models;
    using Metrona.Wt.Identity.Services;
    using Metrona.Wt.Reports.Excel;
    using Metrona.Wt.Reports.Pdf;
    using Metrona.Wt.Service;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    
    internal static class UnityConfig
	{
        private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return LazyContainer.Value;
        }

        private static void RegisterTypes(IUnityContainer container)
		{
            var configuration = new Configuration();
            container.RegisterInstance(configuration);

            /*** Db context  ***/
            container.RegisterType<IEntitiesContext, KlimaContext>(
                new PerResolveLifetimeManager(),
                new InjectionConstructor(configuration.ConnectionString));

            /**** Identity ****/
            container.RegisterType<IUserStore<ApplicationUser, long>, UserStoreService>();
            container.RegisterType<UserManager<ApplicationUser, long>, ApplicationUserManager>();
            container.RegisterType<SignInManager<ApplicationUser, long>, ApplicationSignInManager>();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<IPasswordHasher, Identity.PasswordHasher>();
            
            /*** Repositories ****/
            container.RegisterType<IWetterStationRepository, WetterStationRepository>();
            container.RegisterType<IBundeslandRepository, BundeslandRepository>();
            container.RegisterType<IKlimaRepository, KlimaRepository>();
            container.RegisterType<IMeteoGtzRepository, MeteoGtzRepository>();
            container.RegisterType<IUserRepository, UserRepository>();

            /*** Services ***/
            container.RegisterType<IBundeslandService, BundeslandService>();
            container.RegisterType<IKlimaService, KlimaService>();
            container.RegisterType<IMeteoGtzService, MeteoGtzService>();

            container.RegisterType<IExcelExporter, ExcelExporter>();
            container.RegisterType<IPdfReport, PdfReport>();
            

            //container.RegisterType<WitterungstelegramCreateReport>( new InjectionProperty("BundeslandService"));
		}
	}
}