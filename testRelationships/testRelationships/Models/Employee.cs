using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testRelationships.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AgentID { get; set; }
        public virtual Agent Agent { get; set; }
    }
}