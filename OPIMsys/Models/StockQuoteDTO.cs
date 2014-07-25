using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class StockQuoteDTO
    {
            public DateTime QuoteDate { get; set; }
            public Decimal? Open { get; set; }
            public Decimal? High { get; set; }
            public Decimal? Low { get; set; }
            public Decimal? Change { get; set; }
            public Int32? Volume { get; set; }
            public string MarketCap { get; set; }
            public Decimal? EarningPerShare { get; set; }
            public Decimal? Dividend { get; set; }
            public Decimal? LastPrice { get; set; }
            public string Symbol { get; set; }
            public Decimal? YrHigh { get; set; }
            public Decimal? YrLow { get; set; }
            public Double? AvgVolume { get; set; }
    }
}