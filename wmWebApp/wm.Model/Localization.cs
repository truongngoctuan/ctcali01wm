using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class CultureInfo: Entity<int>
    {
        public string Name { get; set; }
        
        public static class Code
        {
            public const string Default = "Default";
            public const string VN = "VN";
        }
    }

    class LocalizationString : BaseEntity
    {
        [Key, Column(Order = 0)]
        public int CultureInfoId { get; set; }
        [Key, Column(Order = 1)]
        public string Code { get; set; }

        [ForeignKey("CultureInfoId")]
        public virtual CultureInfo CultureInfo { get; set; }
    }
}
