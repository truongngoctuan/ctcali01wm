using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IMultiPurposeListRepository : IGenericIntKeyRepository<MultiPurposeList>
    {
    }

    public class MultiPurposeListRepository : GenericIntKeyRepository<MultiPurposeList>, IMultiPurposeListRepository
    {
        public MultiPurposeListRepository(DbContext context)
              : base(context)
        {

        }
    }
}
