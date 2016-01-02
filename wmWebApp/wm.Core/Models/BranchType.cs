using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Core.Models
{
    public class BranchType
    {
        public BranchType()
        {
            Branchs = new List<Branch>();
            //MerchandiseCategories = new HashSet<MerchandiseCategory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Branch> Branchs { get; set; }

        //public virtual ICollection<MerchandiseCategory> MerchandiseCategories { get; set; }
    }
}