using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class AutoCreatedContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AutoCreatedContext() : base("name=AutoCreatedContext")
        {
        }

        public System.Data.Entity.DbSet<wmWebApp.Models.Agency> Agencies { get; set; }

        public System.Data.Entity.DbSet<wmWebApp.Models.AgencyType> AgencyTypes { get; set; }

        public System.Data.Entity.DbSet<wmWebApp.Models.GoodTypeLvl1> GoodTypeLvl1 { get; set; }

        public System.Data.Entity.DbSet<wmWebApp.Models.GoodTypeLvl2> GoodTypeLvl2 { get; set; }
    }
}
