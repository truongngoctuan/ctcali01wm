﻿namespace wm.Service.Model
{
    public class OrderBranchItem
    {
        public int OrderId { get; set; }
        public int GoodId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string YourNote { get; set; }
        public int RecommenedQuantity { get; set; }
        public int InStock { get; set; }
    }
}
