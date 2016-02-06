using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
}