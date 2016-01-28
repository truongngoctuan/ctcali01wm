using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm.Model
{
    public class Good
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameASCII { get; set; }
        public GoodUnit Unit { get; set; }
    }
}
