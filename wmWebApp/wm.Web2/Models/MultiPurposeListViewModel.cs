namespace wm.Web2.Models
{

    public class MultiPurposeListGoodInExItemViewModel
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string AccoutantCode { get; set; }
        public string UnitName { get; set; }
        public string GoodType { get; set; }
        public int Ranking { get; set; }
    }

    //Include(In) Exclude(Ex) (only) 
    public class MultiPurposeListGoodInExViewModel
    {
        public MultiPurposeListGoodInExItemViewModel[] data { get; set; }
    }

    public class MultiPurposeListBranchInExItemViewModel
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
    }

    //Include(In) Exclude(Ex) (only) 
    public class MultiPurposeListBranchInExViewModel
    {
        public MultiPurposeListGoodInExItemViewModel[] data { get; set; }
    }
}