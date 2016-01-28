using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class OrderBranch
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public OrderBranchStatus Status { get; set; }
    }
}
