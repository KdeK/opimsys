using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace OPIMsys.Models
{
    public class StockSymbol
    {
        public int StockSymbolId { get; set; }
        public string Symbol { get; set; }

        // Navigation properties
        public int CompanyId { get; set; }
        public int MarketId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<StockHistory> StockHistorys { get; set; }
        public virtual ICollection<StockQuote> StockQuotes { get; set; }
        public virtual ICollection<StockDividend> StockDividends { get; set; }
        public virtual Market Market { get; set; }
    }

    public class StockDividend
    {
        public int StockDividendId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime PayableDate { get; set; }
        public DateTime ExDividendDate { get; set; }
        public Double Dividend { get; set; }
        public string Notes { get; set; }

        public int StockSymbolId { get; set; }
        public virtual StockSymbol StockSymbol { get; set; }
    }
    public class StockDividendApi : CompanyDTODividend
    {
        public string Symbol { get; set; }
        public string Market { get; set; }
    }
}