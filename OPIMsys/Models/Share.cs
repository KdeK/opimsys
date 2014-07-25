using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPIMsys.Models
{
    public class Share
    {
        [Key]
        public int ShareId { get; set; }
        [Required]
        public string ClientRemoteId { get; set; }
        [Required]
        public int StockSymbolId { get; set; }
        [ForeignKey("StockSymbolId")]
        public virtual StockSymbol StockSymbol { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool IsDrip { get; set; }
    }

    public class ShareDTO
    {
        public string ClientIdentifier { get; set; }
        public string Market { get; set; }
        public string Symbol { get; set; }
        public List<ShareTransactionDTO> Transactions { get; set; }
    }
    public class ShareTransactionDTO
    {
        public int ShareId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDrip { get; set; }
    }
    public class ShareReturnDTO
    {
        public DateTime ReturnMonth { get; set; }
        public decimal Invested { get; set; }
        public int Shares { get; set; }
        public decimal SharePrice { get; set; }
        public decimal DividendAmt { get; set; }
        public int SharesPurchased { get; set; }
        public decimal TotalReturn { get; set; }
    }
}