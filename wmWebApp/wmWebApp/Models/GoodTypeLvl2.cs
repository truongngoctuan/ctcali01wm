using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class GoodTypeLvl2
    {
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Ghi chú")]
        public string Desc { get; set; }
        [Display(Name = "Thứ tự")]
        public uint Ranking { get; set; }
        [Display(Name = "Loại hàng cấp 1")]
        public string GoodTypeLvl1s { get; set; }
    }
}