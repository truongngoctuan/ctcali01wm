using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace wm.Web2.Modules
{

    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("wm.Repository"))
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();
        }
    }
}