using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class GoodCategory : Entity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<BranchGoodCategory> BranchGoodCategories { get; set; }
    }
}
