using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Core.Models
{
    public class IntermediaryDbContext : IdentityDbContext<ApplicationUser>
    {
        public IntermediaryDbContext(string connectionString)
            : base(connectionString, throwIfV1Schema: false)
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchType> BranchTypes { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
        public DbSet<MerchandiseCategory> MerchandiseCategory { get; set; }
        public DbSet<MerchandiseAccoutantHistory> MerchandiseAccoutantHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Branch>()
                .HasOptional<BranchType>(o => o.BranchType)
                .WithMany(o => o.Branchs)
                .HasForeignKey(o => o.BranchTypeId);

            #region ApplicationUser
            modelBuilder.Entity<ApplicationUser>()
                .HasOptional<Position>(o => o.Position)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.PositionId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional<Branch>(o => o.Branch)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.BranchId);
            #endregion

        }
    }
}
