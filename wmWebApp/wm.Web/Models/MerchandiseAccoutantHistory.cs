using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class MerchandiseAccoutantHistory
    {
        public int Id { get; set; }
        public string NameAccouting { get; set; }
        public DateTime TimeCreated { get; set; }

        public int MerchandiseId { get; set; }
        [ForeignKey("MerchandiseId")]
        public virtual Merchandise Merchandise { get; set; }

        //public int ApplicationUserCreatedId { get; set; }
        //[ForeignKey("ApplicationUserCreatedId")]
        //public ApplicationUser ApplicationUserCreated { get; set; }

    }
}