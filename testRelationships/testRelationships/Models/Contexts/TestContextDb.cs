using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace testRelationships.Models.Contexts
{
    public class TestContextDb : DbContext
    {
        public TestContextDb() : base("DefaultConnection")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Agent> Agents { get; set; }

    }
}