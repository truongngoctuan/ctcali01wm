using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class Good : Entity<int>
    {
        public string Name { get; set; }
        public string NameASCII { get; set; }

        public int UnitId { get; set; }

        public GoodUnit Unit { get; set; }
    }
}
