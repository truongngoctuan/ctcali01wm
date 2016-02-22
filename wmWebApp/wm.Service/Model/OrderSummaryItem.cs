using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Service.Model
{
    // ReSharper disable once InconsistentNaming
    public class SummarizeMainKitchenOrder_Array_Item
    {
        public string Name { get; set; }
        //something else
        public IEnumerable<int> SummaryData { get; set; }
        public int Total { get; set; }
        public int Id { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class SummarizeMainKitchenOrder_Array_ViewModel
    {
        public IEnumerable<SummarizeMainKitchenOrder_Array_Item> Rows { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class SummarizeMainKitchenOrder_Dictionary_Item
    {
        public string Name { get; set; }
        public int Id { get; set; }
        //something else
        public Dictionary<string, int> SummaryData { get; set; }
        public int Total { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class SummarizeMainKitchenOrder_Dictionary_ViewModel
    {
        public IEnumerable<SummarizeMainKitchenOrder_Dictionary_Item> Rows { get; set; }
    }

}
