﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wm.Web2.Models
{

    public class MultiPurposeListInExItemViewModel
    {
        public int GoodId { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
    }

    //Include(In) Exclude(Ex) (only) 
    public class MultiPurposeListInExViewModel
    {
        public MultiPurposeListInExItemViewModel[] data { get; set; }
    }
}