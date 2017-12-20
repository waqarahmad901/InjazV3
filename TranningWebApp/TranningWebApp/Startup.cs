using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TmsWebApp.Startup))]
namespace TmsWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
