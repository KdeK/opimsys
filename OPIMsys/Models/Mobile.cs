using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class Mobile
    {
        public int? id { get; set; }
        public int companyId { get; set; }
        public string type { get; set; }
        public string text { get; set; }
    }
}