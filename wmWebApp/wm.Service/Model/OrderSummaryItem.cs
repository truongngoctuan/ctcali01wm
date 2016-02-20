using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.Model
{
    public class SummarizeMainKitchenOrderItem
    {
        public string Name { get; set; }
        //something else
        public IEnumerable<int> SummaryData { get; set; }
        public int Total { get; set; }
    }

    public class SummarizeMainKitchenOrderViewModel
    {
        public IEnumerable<SummarizeMainKitchenOrderItem> Rows { get; set; }
    }
}
