using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class Branch : Entity<int>
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Branch Name")]
        public string Name { get; set; }

        public virtual ICollection<BranchGoodCategory> BranchGoodCategories { get; set; }
    }

    public class BranchGoodCategory
    {
        [Key, Column(Order = 0)]
        public int BranchID { get; set; }
        [Key, Column(Order = 1)]
        public int GoodCategoryID { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual GoodCategory GoodCategory { get; set; }

        public int Ranking { get; set; }
    }
}
