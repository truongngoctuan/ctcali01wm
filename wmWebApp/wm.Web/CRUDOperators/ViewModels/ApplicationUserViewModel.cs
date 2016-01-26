using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using wm.Web.Models;

namespace wm.Web.CRUDOperators.ViewModels
{
    public class ApplicationUserListDatatableViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string Branch { get; set; }
        public string Position { get; set; }

        public ToolboxListViewModel ToolboxLinks { get; set; }
    }
}