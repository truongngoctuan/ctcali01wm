using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace wm.Core.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser()
        {
            MerchandiseAccoutantHistories = new List<MerchandiseAccoutantHistory>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set;  }
        public string FullNameANSCII { get; set; }
        public string Address { get; set; }

        public int? PositionId { get; set; }
        public virtual Position Position { get; set; }

        public int? BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        public virtual ICollection<MerchandiseAccoutantHistory> MerchandiseAccoutantHistories { get; set; }
    }

}