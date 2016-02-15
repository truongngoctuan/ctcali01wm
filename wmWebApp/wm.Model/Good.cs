using System.ComponentModel.DataAnnotations;

namespace wm.Model
{
    public class Good : Entity<int>
    {
        public string Name { get; set; }
        public string NameASCII { get; set; }
        public string AccountantCode { get; set; }
        public int UnitId { get; set; }

        public GoodUnit Unit { get; set; }

        public GoodType GoodType { get; set; }
    }

    public enum GoodType
    {
        [Display(Name = "Normal")]
        Normal = 0,
        [Display(Name = "KitChen Good")]
        KitChenGood = 1,
        [Display(Name = "Raw KitChen Good")]
        RawKitChenGood = 2,
    }
}
