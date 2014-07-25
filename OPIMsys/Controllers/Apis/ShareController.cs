using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OPIMsys.Models;

namespace OPIMsys.Controllers.Apis
{
    public class ShareController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // GET api/Share
        [HttpGet]
        public IEnumerable<ShareTransactionDTO> GetShares(string clientIdentifier, string symbol, string market)
        {
            
            List<ShareTransactionDTO> returnShares = new List<ShareTransactionDTO>();
            int stockId = db.StockSymbols.Where(a => a.MarketId == db.Markets.Where(c => c.MarketName == market).FirstOrDefault().MarketId).Where(b => b.Symbol == symbol).FirstOrDefault().StockSymbolId;
            var shares = db.Shares.Where(a => a.ClientRemoteId == clientIdentifier).Where(b => b.StockSymbolId == stockId);
            foreach (Share share in shares)
            {
                returnShares.Add(new ShareTransactionDTO { IsDrip = share.IsDrip, Price = share.Price, Quantity = share.Quantity, ShareId = share.ShareId, TransactionDate = share.TransactionDate });
            }
            return returnShares.ToArray();
        }

        // POST api/Share
        [HttpPost]
        public HttpResponseMessage PostShare(ShareDTO share)
        {
            //string symbol, string market, 
           
            Share shareDO = new Share();
            shareDO.ClientRemoteId = share.ClientIdentifier;
            shareDO.StockSymbolId = db.StockSymbols.Where(a => a.MarketId == db.Markets.Where(c => c.MarketName == share.Market).FirstOrDefault().MarketId).Where(b => b.Symbol == share.Symbol).FirstOrDefault().StockSymbolId;
            shareDO.IsDrip = share.Transactions[0].IsDrip;
            shareDO.Price = share.Transactions[0].Price;
            shareDO.Quantity = share.Transactions[0].Quantity;
            shareDO.TransactionDate = share.Transactions[0].TransactionDate;
                
            if (ModelState.IsValid)
            {
                db.Shares.Add(shareDO);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, shareDO);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = shareDO.ShareId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        
        // DELETE api/Share/5
        public HttpResponseMessage DeleteShare(int id)
        {
            Share share = db.Shares.Find(id);
            if (share == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Shares.Remove(share);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, share);
        }

        [HttpGet]
        [ActionName("TotalReturn")]
        public IEnumerable<ShareReturnDTO> TotalReturn(string clientIdentifier, string symbol, string market)
        {
            List<ShareReturnDTO> shareReturn = new List<ShareReturnDTO>();
            int stockId = db.StockSymbols.Where(a => a.MarketId == db.Markets.Where(c => c.MarketName == market).FirstOrDefault().MarketId).Where(b => b.Symbol == symbol).FirstOrDefault().StockSymbolId;
            var shares = db.Shares.Where(a => a.ClientRemoteId == clientIdentifier).Where(b => b.StockSymbolId == stockId).OrderBy(a => a.TransactionDate);
            DateTime currentDate = shares.First().TransactionDate;
            var dividends = db.StockDividends.Where(a => a.StockSymbolId==stockId).Where(a=>a.ExDividendDate>currentDate);
            int runningSharesPurchased = 0;
            decimal runningDividendAmt = 0;
            while (currentDate < DateTime.Now.AddMonths(-1))
            {
                DateTime startMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0,0,0);
                DateTime endMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month),23,59,59);
                decimal dividendAmt = 0;
                int sharesPurchased = 0;
                if (dividends.Where(a => a.ExDividendDate > startMonth).Where(a => a.ExDividendDate < endMonth).Count() > 0)
                {
                    //Deal with non-drip first 


                    foreach (var distribs in dividends.Where(a => a.ExDividendDate > startMonth).Where(a => a.ExDividendDate < endMonth))
                    {
                        if(shares.Where(a => a.TransactionDate <= distribs.ExDividendDate).Where(a => a.IsDrip == false).Count() > 0)
                            dividendAmt += shares.Where(a => a.TransactionDate <= distribs.ExDividendDate).Where(a => a.IsDrip == false).Sum(a => a.Quantity) * Convert.ToDecimal(distribs.Dividend);
                        var stockprice = db.StockHistories.Where(a => a.StockSymbolId == stockId).Where(a => a.HistoryDate <= distribs.RecordDate).OrderByDescending(a => a.HistoryDate).FirstOrDefault();
                        decimal distribStockPrice = Convert.ToDecimal(stockprice.Close);
                        if (shares.Where(a => a.TransactionDate <= distribs.ExDividendDate).Where(a => a.IsDrip == true).Count() > 0)
                            //((shares.Where(a => a.TransactionDate <= distribs.ExDividendDate).Where(a => a.IsDrip == true).Sum(a => a.Quantity) + runningSharesPurchased + sharesPurchased) * Convert.ToDecimal(distribs.Dividend)) / (distribStockPrice * 0.95))

                            sharesPurchased += Convert.ToInt16(decimal.Floor(Convert.ToDecimal(((shares.Where(a => a.TransactionDate <= distribs.ExDividendDate).Where(a => a.IsDrip == true).Sum(a => a.Quantity) + runningSharesPurchased + sharesPurchased) * Convert.ToDecimal(distribs.Dividend)) / (distribStockPrice * 0.95m))));
                    }

                    runningDividendAmt += dividendAmt;
                    runningSharesPurchased += sharesPurchased;
                }
                //Return
                int shareEOM = shares.Where(a => a.TransactionDate <= endMonth).Sum(a => a.Quantity) + runningSharesPurchased;
                decimal sharePrice =0;
                if(db.StockHistories.Where(a => a.StockSymbolId == stockId).Where(a => a.HistoryDate <= endMonth).OrderBy(a => a.HistoryDate).Count() > 0)
                    sharePrice = Convert.ToDecimal(db.StockHistories.Where(a => a.StockSymbolId == stockId).Where(a => a.HistoryDate <= endMonth).OrderByDescending(a => a.HistoryDate).First().AdjClose);
                decimal invested = shares.Where(a => a.TransactionDate <= endMonth).Sum(a => a.Price * a.Quantity);

                decimal totalReturn = (shareEOM * sharePrice) + runningDividendAmt - invested;

                shareReturn.Add( new ShareReturnDTO {
                    ReturnMonth = endMonth,
                    Invested = decimal.Round(invested, 2),
                    Shares = shareEOM,
                    SharePrice = sharePrice,
                    DividendAmt = decimal.Round(dividendAmt, 2),
                    SharesPurchased = sharesPurchased,
                    TotalReturn = decimal.Round(totalReturn, 2)
                });
                currentDate = currentDate.AddMonths(1);    
            }
            
            return shareReturn;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}