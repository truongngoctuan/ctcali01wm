using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using AutoMapper.Mappers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using wm.Model;
using wm.Web2.App_Start;
using wm.Web2.Modules;

[assembly: OwinStartupAttribute(typeof(wm.Web2.Startup))]
namespace wm.Web2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //https://www.talksharp.com/configuring-autofac-to-work-with-the-aspnet-identity-framework-in-mvc-5
            //Autofac Configuration
            var builder = new ContainerBuilder();

            // REGISTER DEPENDENCIES
            builder.RegisterType<wmContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new ServiceModule());

            //automapper
            //current automapper is 4.1.1, 4.2.0 change configuration
            //http://stackoverflow.com/questions/31321148/exception-when-i-try-to-combine-autofac-with-automappers-imappingengine
            //http://www.paraesthesia.com/archive/2014/03/25/automapper-autofac-web-api-and-per-request-dependency-lifetime-scopes.aspx/

            builder.RegisterType<AutoMapperWebProfiler>().As<Profile>();

            builder.Register(ctx => new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers))
                .AsImplementedInterfaces()
                .SingleInstance()
                .OnActivating(x =>
                {
                    foreach (var profile in x.Context.Resolve<IEnumerable<Profile>>())
                    {
                        x.Instance.AddProfile(profile);
                    }
                });

            builder.RegisterType<MappingEngine>().As<IMappingEngine>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ConfigureAuth(app);
        }
    }
}
