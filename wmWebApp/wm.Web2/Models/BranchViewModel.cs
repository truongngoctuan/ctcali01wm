using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Web2.Models
{
    public class BranchInExItemViewModel
    {
        public int CategoryId { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
    }

    //Branch Include(In) Exclude(Ex) (only) 
    public class BranchInExViewModel
    {
        public BranchInExItemViewModel[] data { get; set; }
    }

}