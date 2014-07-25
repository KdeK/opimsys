using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class StockQuote
    {
        public int StockQuoteId { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime? TradeDate { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Change { get; set; }
        public int Volume { get; set; }
        public string MarketCap { get; set; }
        public decimal? EarningPerShare { get; set; }
        public decimal? Dividend { get; set; }
        public decimal LastPrice { get; set; }

        public int StockSymbolId { get; set; }
        public virtual StockSymbol StockSymbol { get; set; }
    }
}