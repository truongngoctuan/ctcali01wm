using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public enum AgencyTypeEnum
    {
        CHI_NHANH_FULL,
        CHI_NHANH_KO_BAR,
        BEP_TONG,
        KHO_TONG
    };
    public class AgencyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AgencyTypeEnum SystemType { get; set; }
        public virtual ICollection<GoodTypeLvl1> Courses { get; set; }
    } 
}