using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Web2.Models
{

    public class MultiPurposeListInExItemViewModel
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string AccoutantCode { get; set; }
        public string UnitName { get; set; }
        public string GoodType { get; set; }
        public int Ranking { get; set; }
    }

    //Include(In) Exclude(Ex) (only) 
    public class MultiPurposeListInExViewModel
    {
        public MultiPurposeListInExItemViewModel[] data { get; set; }
    }
}