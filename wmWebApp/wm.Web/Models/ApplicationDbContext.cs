using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using wm.Core.Models;

namespace wm.Web.Models
{
    public class ApplicationDbContext
        : IntermediaryDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    //    : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }

    //    public DbSet<Branch> Brands { get; set; }
    //    public DbSet<BranchType> BrandType { get; set; }
    //    public DbSet<Merchandise> Merchandise { get; set; }
    //    public DbSet<MerchandiseCategory> MerchandiseCategory { get; set; }
    //    public DbSet<MerchandiseAccoutantHistory> MerchandiseAccoutantHistory { get; set; }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);
    //        modelBuilder.Entity<Branch>()
    //            .HasOptional<BranchType>(o => o.BranchType)
    //            .WithMany(o => o.Branchs)
    //            .HasForeignKey(o => o.BranchTypeId);
    //    }
    //}

}