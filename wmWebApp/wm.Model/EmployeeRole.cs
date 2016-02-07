using System.ComponentModel.DataAnnotations;

namespace wm.Model
{
    public enum EmployeeRole
    {
        [Display(Name = "StaffBranch")]
        StaffBranch = 0,
        [Display(Name = "Manager")]
        Manager = 1,
        [Display(Name = "WarehouseKeeper")]
        WarehouseKeeper = 2,
        [Display(Name = "Admin")]
        Admin = 5,
    }
}