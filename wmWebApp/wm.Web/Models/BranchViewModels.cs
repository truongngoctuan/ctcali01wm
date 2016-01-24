using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using wm.Web.CRUDOperators.ViewModels;

namespace wm.Web.Models
{
    public class BranchListViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string BranchTypeName { get; set; }

        public ToolboxListViewModel ToolboxLinks { get; set; }
    }
}