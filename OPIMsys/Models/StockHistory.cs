using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class StockHistory
    {
        public int StockHistoryId { get; set; }
        public DateTime HistoryDate { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public decimal AdjClose { get; set; }

        public int StockSymbolId { get; set; }
        public virtual StockSymbol StockSymbol { get; set; }
    }
}