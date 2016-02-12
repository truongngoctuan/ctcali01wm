using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wm.Model
{
    public class Order: AuditableEntity<int>
    {
        public int BranchId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDay { get; set; }

        public int Priority { get; set; }//higher is not allow to be modified by lower
        public virtual ICollection<OrderGood> OrderGoods { get; set; }
    }

    public class OrderGood : BaseEntity
    {
        [Key, Column(Order = 0)]
        public int OrderId { get; set; }
        [Key, Column(Order = 1)]
        public int GoodId { get; set; }

        [Key, Column(Order = 2)]
        public int UserId { get; set; } //no need to make too many relations
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }


        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("GoodId")]
        public virtual Good Good { get; set; }

        public int Quantity { get; set; }
        public string Note { get; set; }
    }
}
