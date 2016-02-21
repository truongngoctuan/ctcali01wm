using System;
using System.Collections.Generic;
using wm.Model;
using wm.Service.Model;

namespace wm.Web2.Models
{
	public class OrderViewModel
	{
        public OrderBranchItem[] data { get; set; }
    }

    public class CreateOrderByStaffViewModel
    {
        public string employeeId { get; set; }
        public int branchId { get; set; }
        public DateTime orderDay { get; set; }
    }

    //for partial view
    // ReSharper disable once InconsistentNaming
    public class StaffEditOrder_GoodCategoryViewModel
    {
        public IEnumerable<GoodCategory> GoodCategories { get; set; }
        public string EditAction { get; set; }
        public int orderId { get; set; }
    }
}