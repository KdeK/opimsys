using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class Market
    {
        public int MarketId { get; set; }
        public string MarketName { get; set; }
        public string MarketOpen { get; set; }
        public string MarketClose { get; set; }

        // Navigation properties
        public virtual ICollection<StockSymbol> StockSymbols { get; set; }
    }
}