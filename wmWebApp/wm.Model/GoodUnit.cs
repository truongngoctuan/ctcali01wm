using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wm.Model
{
    public class GoodUnit : Entity<int>
    {
        public GoodUnit()
        {
            Goods = new List<Good>();
        }

        public string Name { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}