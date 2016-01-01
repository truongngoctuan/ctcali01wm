using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandType> BrandType { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
        public DbSet<MerchandiseCategory> MerchandiseCategory { get; set; }
        public DbSet<MerchandiseAccoutantHistory> MerchandiseAccoutantHistory { get; set; }

    }
}