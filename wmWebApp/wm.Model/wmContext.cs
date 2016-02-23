using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace wm.Model
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class wmContext : IdentityDbContext<ApplicationUser>
    {
        public wmContext()
            : base("Name=DefaultConnection")
        {

        }
        public static wmContext Create()
        {
            return new wmContext();
        }

        public DbSet<Branch> Branches { get; set; }//testing TEntity
        public DbSet<GoodCategory> GoodCategories { get; set; }
        public DbSet<BranchGoodCategory> BranchGoodCategories { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<GoodCategoryGood> GoodCategoryGoods { get; set; }
        public DbSet<GoodUnit> GoodUnits { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderGood> OrderGoods { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }//testing auditable

        public DbSet<MultiPurposeList> MultiPurposeLists { get; set; }
        public DbSet<MultiPurposeListGood> MultiPurposeListGoods { get; set; }


        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else {
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one-to-many
            modelBuilder.Entity<Good>()
                        .HasRequired<GoodUnit>(s => s.Unit)
                        .WithMany(s => s.Goods)
                        .HasForeignKey(s => s.UnitId)
                        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
