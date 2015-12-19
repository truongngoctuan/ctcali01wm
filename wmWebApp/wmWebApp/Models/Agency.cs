using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class Agency
    {
        public int Id { get; set; }
        [Display(Name = "Tên Chi Nhánh")]
        public string Name { get; set; }
        [Display(Name = "Chi Tiết")]
        public string Desc { get; set; }
        [Display(Name = "Thứ tụ hiển thị")]
        public uint Ranking { get; set; }
        [Display(Name = "Loại Chi Nhánh")]
        public AgencyType Type { get; set; }
    }
}