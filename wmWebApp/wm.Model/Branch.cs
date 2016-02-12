using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wm.Model
{
    public class Branch : Entity<int>
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Branch Name")]
        public string Name { get; set; }
        public BranchType BranchType { get; set; }

        public virtual ICollection<BranchGoodCategory> BranchGoodCategories { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public enum BranchType
    {
        [Display(Name = "Normal")]
        Normal = 0,
        [Display(Name = "Main Kitchen")]
        MainKitchen = 1,
        [Display(Name = "Main Warehouse")]
        MainWarehouse = 2,
    }

    public class BranchGoodCategory : BaseEntity
    {
        [Key, Column(Order = 0)]
        public int BranchId { get; set; }
        [Key, Column(Order = 1)]
        public int GoodCategoryId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        [ForeignKey("GoodCategoryId")]
        public virtual GoodCategory GoodCategory { get; set; }

        public int Ranking { get; set; }
    }
}
