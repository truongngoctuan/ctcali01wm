using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class Agency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public uint Ranking { get; set; }
        public AgencyType Type { get; set; }
    }
}