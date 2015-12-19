using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class WorldContext: DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        public WorldContext(): base() 
    {
            Database.SetInitializer<WorldContext>(new WorldContextInitializer());

        }
    }
}