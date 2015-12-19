using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wmWebApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public Agency Agency { get; set; }
    }
}