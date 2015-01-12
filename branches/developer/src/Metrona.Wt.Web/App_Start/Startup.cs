using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Metrona.Wt.Web.Startup))]
namespace Metrona.Wt.Web
{
    using Metrona.Wt.Identity;

    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            IdentityStartup.ConfigureAuth(app);
        }
    }
}
