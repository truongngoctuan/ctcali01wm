using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class BrandType
    {
        public BrandType()
        {
            Brands = new List<Brand>();
            MerchandiseCategories = new HashSet<MerchandiseCategory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }

        public virtual ICollection<MerchandiseCategory> MerchandiseCategories { get; set; }
    }
}