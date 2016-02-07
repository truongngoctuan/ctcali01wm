using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class Employee: Entity<int>
    {
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PlainPassword { get; set; } //when user reset password by themself, this field is emptied
        public EmployeeRole Role { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

    }
}
