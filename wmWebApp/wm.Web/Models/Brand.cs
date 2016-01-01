using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public int BrandTypeId { get; set; }
        [ForeignKey("BrandTypeId")]
        public virtual BrandType BrandType { get; set; }
    }
}