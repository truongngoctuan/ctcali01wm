using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wmWebApp.Startup))]
namespace wmWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
