﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGoodCategoryRepository : IGenericIntKeyRepository<GoodCategory>
    {
    }

    public class GoodCategoryRepository : GenericIntKeyRepository<GoodCategory>, IGoodCategoryRepository
    {
        public GoodCategoryRepository(DbContext context)
              : base(context)
        {

        }
    }
}
