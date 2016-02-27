using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class MultiPurposeList : Entity<int>
    {
        public string Name { get; set; }
    }

    public class MultiPurposeListGood : BaseEntity
    {
        [Key, Column(Order = 0)]
        public int MultiPurposeListId { get; set; }
        [Key, Column(Order = 1)]
        public int GoodId { get; set; }

        [ForeignKey("MultiPurposeListId")]
        public virtual MultiPurposeList MultiPurposeList { get; set; }
        [ForeignKey("GoodId")]
        public virtual Good Good { get; set; }

        public int Ranking { get; set; }
    }

    public class MultiPurposeListBranch : BaseEntity
    {
        [Key, Column(Order = 0)]
        public int MultiPurposeListId { get; set; }
        [Key, Column(Order = 1)]
        public int BranchId { get; set; }

        [ForeignKey("MultiPurposeListId")]
        public virtual MultiPurposeList MultiPurposeList { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public int Ranking { get; set; }
    }
}
