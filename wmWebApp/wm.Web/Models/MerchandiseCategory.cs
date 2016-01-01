using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class MerchandiseCategory
    {
        public MerchandiseCategory()
        {
            Merchandises = new List<Merchandise>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public MerchandiseCategory Parent { get; set; }

        public virtual ICollection<Merchandise> Merchandises { get; set; }

        public virtual ICollection<BrandType> BrandTypes { get; set; }
    }
}