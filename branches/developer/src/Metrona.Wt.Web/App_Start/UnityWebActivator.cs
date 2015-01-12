using System.Web;

using Metrona.Wt.Web.App_Start;

[assembly: WebActivator.PostApplicationStartMethod(typeof(UnityWebActivator), "PostStart")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(UnityWebActivator), "Shutdown")]

namespace Metrona.Wt.Web.App_Start
{
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using Unity.Mvc5;
    using Unity.WebForms;

    public class UnityWebActivator
    {
        internal static void PostStart()
        {
            var container = UnityConfig.GetConfiguredContainer();

            /*** For WebForms ****/
            HttpContext.Current.Application.SetContainer(container);

            /*** For MVC ****/
            //FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            //FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        internal static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}