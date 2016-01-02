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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOptional<Agent>(o => o.Agent)
                .WithMany(o => o.Employees)
                .HasForeignKey(o => o.AgentID);
        }
    }
}