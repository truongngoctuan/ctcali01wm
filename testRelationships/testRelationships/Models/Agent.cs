using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testRelationships.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}