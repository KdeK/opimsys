using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class StockHistoryDTO
    {
        public string Date { get; set; }
        public Decimal Open { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Close { get; set; }
        public Double Volume { get; set; }
    }
}