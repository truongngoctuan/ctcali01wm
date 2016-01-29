using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Web2.Models
{
    public class BranchGoodCategoryViewModel
    {
        public int CategoryId { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }

    }
}