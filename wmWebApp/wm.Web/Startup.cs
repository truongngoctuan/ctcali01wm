using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wm.Web.Startup))]
namespace wm.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
