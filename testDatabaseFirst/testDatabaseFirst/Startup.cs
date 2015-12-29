using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testDatabaseFirst.Startup))]
namespace testDatabaseFirst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
