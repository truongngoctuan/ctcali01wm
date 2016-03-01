using System.Collections.Generic;

namespace wm.Service.Model
{
    public class OrderMainKitchenItem
    {
        public int OrderId { get; set; }
        public int GoodId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string YourNote { get; set; }
        public int RecommenedQuantity { get; set; }
        public int InStock { get; set; }
        public int QuantityFromBranch { get; set; }

        public Dictionary<string, int> Details { get; set; } //branchId, value
    }
}
