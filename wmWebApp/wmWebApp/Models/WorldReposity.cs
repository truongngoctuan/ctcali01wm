using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class WorldReposity : IWorldReposity
    {
        private WorldContext _context;

        public WorldReposity(WorldContext context)
        {
            _context = context;
        }
        public IEnumerable<Agency> GetAllAgency()
        {
            return _context.Agencies.OrderBy(t => t.Ranking).ToList();

        }
    }
}