using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public uint RecommendedQuantity { get; set; }
    }

    public class PlacingOrderViewModel
    {
        public OrderItemViewModel[] data { get; set; }
    }
}