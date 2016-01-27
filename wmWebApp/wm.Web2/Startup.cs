using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wm.Web2.Startup))]
namespace wm.Web2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
