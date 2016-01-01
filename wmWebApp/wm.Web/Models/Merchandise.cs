using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public enum MerchandiseType
    {
        PRODUCT_KITCHEN,
        RAW_FROM_SUPPLIER
    }

    public class Merchandise
    {
        public Merchandise()
        {
            MerchandiseAccoutantHistory = new List<MerchandiseAccoutantHistory>();
        }
        public int Id { get; set; }
        public string NameUnicode { get; set; }
        public string NameAnsi { get; set; }
        public string Measurement { get; set; }
        public MerchandiseType MerchandiseType { get; set; }

        public int MerchandiseCategoryId { get; set; }
        [ForeignKey("MerchandiseCategoryId")]
        public virtual MerchandiseCategory MerchandiseCategory { get; set; }

        public virtual ICollection<MerchandiseAccoutantHistory> MerchandiseAccoutantHistory { get; set; }

    }


}