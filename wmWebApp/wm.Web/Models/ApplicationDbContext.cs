﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using wm.Core.Models;
using wm.Web.Migrations.ApplicationDbContext;

namespace wm.Web.Models
{
    public class ApplicationDbContext
        : IntermediaryDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
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

    //    public DbSet<Branch> Branches { get; set; }
    //    public DbSet<BranchType> BranchTypes { get; set; }
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