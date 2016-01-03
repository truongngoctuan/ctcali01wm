using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wm.Web.Models
{
    public class BranchListViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string BranchTypeName { get; set; }

        public _ToolboxListViewModel ToolboxLinks { get; set; }
    }
}