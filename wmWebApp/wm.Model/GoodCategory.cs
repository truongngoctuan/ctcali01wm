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

    public class GoodCategoryGood : BaseEntity
    {
        [Key, Column(Order = 0)]
        public int GoodCategoryId { get; set; }
        [Key, Column(Order = 1)]
        public int GoodId { get; set; }

        [ForeignKey("GoodCategoryId")]
        public virtual GoodCategory GoodCategory { get; set; }
        [ForeignKey("GoodId")]
        public virtual Good Good { get; set; }

        public int Ranking { get; set; }
    }
}
