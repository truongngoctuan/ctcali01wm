using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wm.Model
{
    public class CultureInfoCode
    {
        public const string Default = "Default";
        public const string VN = "VN";
    }

    public class LocalizationString : BaseEntity
    {
        [Key, Column(Order = 0)]
        public string CultureInfoString { get; set; }
        [Key, Column(Order = 1)]
        public string Code { get; set; }

        public string Value { get; set; }
    }
}
