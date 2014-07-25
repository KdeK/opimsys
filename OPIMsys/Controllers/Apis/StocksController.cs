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
using System.IO;


namespace OPIMsys.Controllers.Apis
{
    public class StocksController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // GET api/Stocks
        public IEnumerable<StockQuote> GetStockQuotes()
        {
            var stockquotes = db.StockQuotes.Include(s => s.StockSymbol);
            return stockquotes.AsEnumerable();
        }

        private StockQuoteDTO GetStockQuote(int id)
        {
            StockQuote stockquote = (from x in db.StockQuotes
                                     where x.StockSymbolId == id
                                     orderby x.QuoteDate descending
                                     select x).First();
            if (stockquote == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            DateTime yearago = DateTime.Now.AddYears(-1);

            var year = db.StockHistories.Where(x => x.StockSymbolId == id).Where(b => b.HistoryDate> yearago);
            decimal yrHigh = 0;
            decimal yrLow = 0;
            double yrVol = 0;
            if (year.Count() > 0)
            {
                yrHigh = year.Max(a => a.High);
                yrLow = year.Min(a => a.Low);
                yrVol = year.Average(a => a.Volume);
            }
            //return stockquote;
            return new StockQuoteDTO()
            {
                Change = stockquote.Change,
                MarketCap = stockquote.MarketCap,
                Dividend = stockquote.Dividend,
                EarningPerShare = stockquote.EarningPerShare,
                High = stockquote.High,
                LastPrice = stockquote.LastPrice,
                Low = stockquote.Low,
                Open = stockquote.Open,
                QuoteDate = stockquote.QuoteDate,
                Symbol = stockquote.StockSymbol.Symbol,
                Volume = stockquote.Volume,
                YrHigh = yrHigh,
                YrLow = yrLow,
                AvgVolume = yrVol
            };

        }
        [HttpGet]
        public StockQuoteDTO Quote(string symbol, string market)
        {
            int marketId = db.Markets.First(m=> m.MarketName == market).MarketId;
            int stockSymbolId = db.StockSymbols.First(x => x.Symbol == symbol && x.MarketId == marketId).StockSymbolId;

            return GetStockQuote(stockSymbolId);
        }

        [HttpGet]
        public StockHistoryDTO[] History(string symbol, string market, int? months)
        {
            int marketId = db.Markets.First(m => m.MarketName == market).MarketId;
            int stockSymbolId = db.StockSymbols.First(x => x.Symbol == symbol && x.MarketId == marketId).StockSymbolId;

            return StockHistory(stockSymbolId, months);
        }




        private long ConvertToUnix(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        private StockHistoryDTO[] StockHistory(int id, int? months)
        {
            List<StockHistory> stockHistory;
            int days = 5*12*25;
            if (months.HasValue)
            {
                days = (int)months * 25;
            }
            if(days != 0)
            {
                stockHistory = (from s in db.StockHistories
                                where s.StockSymbolId == id
                                orderby s.HistoryDate descending
                                select s).Take(days).ToList();
            }
            else
            {
                stockHistory = (from s in db.StockHistories
                                where s.StockSymbolId == id
                                orderby s.HistoryDate descending
                                select s).ToList();
            }
            List<StockHistoryDTO> retList = stockHistory.ConvertAll(x =>
                new StockHistoryDTO { Close = x.Close, Date = (x.HistoryDate.ToString("MM/dd/yyyy HH:mm:ss")), High = x.High, Low = x.Low, Open = x.Open, Volume = x.Volume });
            retList.Reverse();
            return retList.ToArray();

        }
        
        [HttpGet]
        [ActionName("SyncHistory")]
        public HttpStatusCode SyncHistory(int id)
        {
            OPIMsysContext stockquotes = new OPIMsysContext();
            var stocks = (from c in stockquotes.StockSymbols
                          select c.StockSymbolId).ToArray();

            foreach (int ids in stocks)
            {
                SyncSymbolHistory(ids);
            }
            return HttpStatusCode.OK;
        }

        private void SyncSymbolHistory(int id)
        {
            OPIMsysContext stockquotes = new OPIMsysContext();
            var stock = stockquotes.StockSymbols.FirstOrDefault((s) => s.StockSymbolId == id);
            if (stock != null)
            {
                var market = (from s in stockquotes.Markets
                                 where s.MarketId == stock.MarketId
                                 select s).FirstOrDefault();

                switch (market.MarketName)
                {
                    
                    case "NYSE":
                        {
                            int i = 1;
                            int errorcnt = 0;
                            int skipcnt = 0;
                            while (i >= 0)
                            {
                                int skip = 0;
                                DateTime today = DateTime.Today;
                                today = today.AddDays(-1 * i);
                                if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
                                    skip++;

                                //Check to see if the history exists
                                int daycnt = (from s in stockquotes.StockHistories
                                              where s.HistoryDate == today && s.StockSymbolId == stock.StockSymbolId
                                              select s).Count();
                                if (daycnt > 0)
                                    skip++;
                                skipcnt += skip;
                                if (skipcnt > 2)
                                    i = -3;

                                //It does then go to the next
                                if (skip == 0)
                                {

                                    string url = "http://ichart.finance.yahoo.com/table.csv?s=" + stock.Symbol + "";
                                    url = url + "&a=" + String.Format("{0:00}", (today.Month - 1)) + "&b=" + today.Day.ToString("D2") + "&c=" + today.Year + "&d=" + String.Format("{0:00}", (today.Month - 1)) + "&e=" + today.Day.ToString("D2") + "&f=" + today.Year + "&g=d&ignore=.csv";

                                    try
                                    {
                                        WebRequest request = WebRequest.Create(url);
                                        WebResponse response = request.GetResponse();
                                        Stream dataStream = response.GetResponseStream();
                                        StreamReader sr = new StreamReader(dataStream);
                                        string line = sr.ReadLine();
                                        line = sr.ReadLine();
                                        List<string> returnVal = line.Split(',').ToList();
                                        //Is the data valid
                                        if (returnVal.Count == 7)
                                        {
                                            StockHistory stockHistory = new StockHistory();
                                            stockHistory.HistoryDate = DateTime.Parse(returnVal[0]);
                                            stockHistory.Open = Decimal.Parse(returnVal[1]);
                                            stockHistory.High = Decimal.Parse(returnVal[2]);
                                            stockHistory.Low = Decimal.Parse(returnVal[3]);
                                            stockHistory.Close = Decimal.Parse(returnVal[4]);
                                            stockHistory.Volume = long.Parse(returnVal[5]);
                                            if (returnVal.Count > 6)
                                                stockHistory.AdjClose = Decimal.Parse(returnVal[6]);
                                            stockHistory.StockSymbolId = stock.StockSymbolId;
                                            if (stockquotes.StockHistories.Count((c) => c.StockSymbolId == stock.StockSymbolId && c.HistoryDate == stockHistory.HistoryDate) == 0)
                                                stockquotes.StockHistories.Add(stockHistory);
                                            stockquotes.SaveChanges();
                                            errorcnt = 0;
                                        }
                                    }
                                    catch (WebException ex)
                                    {
                                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                                        {
                                            var resp = (HttpWebResponse)ex.Response;
                                            if (resp.StatusCode == HttpStatusCode.NotFound)
                                            {
                                                errorcnt++;
                                                if (errorcnt > 7)
                                                    i = -3;
                                            }
                                        }
                                    }
                                }
                                i++;
                            }

                            break;
                        }
                    case "TSX":
                        {
                            int i = 1;
                            int errorcnt = 0;
                            int skipcnt = 0;
                            while (i >= 0)
                            {
                                int skip = 0;
                                DateTime today = DateTime.Today;
                                today = today.AddDays(-1 * i);
                                if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
                                    skip++;
                                
                                //Check to see if the history exists
                                int daycnt = (from s in stockquotes.StockHistories
                                              where s.HistoryDate == today && s.StockSymbolId == stock.StockSymbolId
                                              select s).Count();
                                if(daycnt > 0)
                                    skip++;
                                skipcnt += skip;
                                if (skipcnt > 2)
                                    i = -3;
                                //It does then go to the next
                                if(skip  == 0)
                                {
                                    string symbol = stock.Symbol;
                                    List<string> protectSymbols = new List<string>();
                                    protectSymbols.Add("EGL-UN");
                                    protectSymbols.Add("AET-UN");
                                    protectSymbols.Add("PLT-UN");
                                    if(!protectSymbols.Contains(symbol))
                                        symbol = symbol.Replace('-', '.');

                                    string url = "http://ichart.finance.yahoo.com/table.csv?s=" + symbol + ".TO";
                                    url = url + "&a=" + String.Format("{0:00}", (today.Month - 1)) + "&b=" + today.Day.ToString("D2") + "&c=" + today.Year + "&d=" + String.Format("{0:00}", (today.Month - 1)) + "&e=" + today.Day.ToString("D2") + "&f=" + today.Year + "&g=d&ignore=.csv";

                                    try
                                    {
                                        WebRequest request = WebRequest.Create(url);
                                        WebResponse response = request.GetResponse();
                                        Stream dataStream = response.GetResponseStream();
                                        StreamReader sr = new StreamReader(dataStream);
                                        string line = sr.ReadLine();
                                        line = sr.ReadLine();
                                        List<string> returnVal = line.Split(',').ToList();
                                        //Is the data valid
                                        if (returnVal.Count == 7)
                                        {
                                            StockHistory stockHistory = new StockHistory();
                                            stockHistory.HistoryDate = DateTime.Parse(returnVal[0]);
                                            stockHistory.Open = Decimal.Parse(returnVal[1]);
                                            stockHistory.High = Decimal.Parse(returnVal[2]);
                                            stockHistory.Low = Decimal.Parse(returnVal[3]);
                                            stockHistory.Close = Decimal.Parse(returnVal[4]);
                                            stockHistory.Volume = long.Parse(returnVal[5]);
                                            if (returnVal.Count > 6)
                                                stockHistory.AdjClose = Decimal.Parse(returnVal[6]);
                                            stockHistory.StockSymbolId = stock.StockSymbolId;
                                            if (stockquotes.StockHistories.Count((c) => c.StockSymbolId == stock.StockSymbolId && c.HistoryDate == stockHistory.HistoryDate) == 0)
                                                stockquotes.StockHistories.Add(stockHistory);
                                            stockquotes.SaveChanges();
                                            errorcnt = 0;
                                        }
                                    }
                                    catch (WebException ex)
                                    {
                                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                                        {
                                            var resp = (HttpWebResponse)ex.Response;
                                            if (resp.StatusCode == HttpStatusCode.NotFound)
                                            {
                                                errorcnt++;
                                                if (errorcnt > 25)
                                                    i = -3;
                                            }
                                        }
                                    }
                                }
                                i++;
                            }
                            
                            break;
                        }
                    case "TSX.V":
                        {
                            int i = 1;
                            int errorcnt = 0;
                            int skipcnt = 0;
                            while (i >= 0)
                            {
                                int skip = 0;
                                DateTime today = DateTime.Today;
                                today = today.AddDays(-1 * i);
                                if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
                                    skip++;

                                //Check to see if the history exists
                                int daycnt = (from s in stockquotes.StockHistories
                                              where s.HistoryDate == today && s.StockSymbolId == stock.StockSymbolId
                                              select s).Count();
                                if (daycnt > 0)
                                    skip++;
                                skipcnt += skip;
                                if (skipcnt > 2)
                                    i = -3;
                                //It does then go to the next
                                if (skip == 0)
                                {

                                    string url = "http://ichart.finance.yahoo.com/table.csv?s=" + stock.Symbol + ".V";
                                    url = url + "&a=" + String.Format("{0:00}", (today.Month - 1)) + "&b=" + today.Day.ToString("D2") + "&c=" + today.Year + "&d=" + String.Format("{0:00}", (today.Month - 1)) + "&e=" + today.Day.ToString("D2") + "&f=" + today.Year + "&g=d&ignore=.csv";

                                    try
                                    {
                                        WebRequest request = WebRequest.Create(url);
                                        WebResponse response = request.GetResponse();
                                        Stream dataStream = response.GetResponseStream();
                                        StreamReader sr = new StreamReader(dataStream);
                                        string line = sr.ReadLine();
                                        line = sr.ReadLine();
                                        List<string> returnVal = line.Split(',').ToList();
                                        //Is the data valid
                                        if (returnVal.Count == 7)
                                        {
                                            StockHistory stockHistory = new StockHistory();
                                            stockHistory.HistoryDate = DateTime.Parse(returnVal[0]);
                                            stockHistory.Open = Decimal.Parse(returnVal[1]);
                                            stockHistory.High = Decimal.Parse(returnVal[2]);
                                            stockHistory.Low = Decimal.Parse(returnVal[3]);
                                            stockHistory.Close = Decimal.Parse(returnVal[4]);
                                            stockHistory.Volume = long.Parse(returnVal[5]);
                                            if (returnVal.Count > 6)
                                                stockHistory.AdjClose = Decimal.Parse(returnVal[6]);
                                            stockHistory.StockSymbolId = stock.StockSymbolId;
                                            if (stockquotes.StockHistories.Count((c) => c.StockSymbolId == stock.StockSymbolId && c.HistoryDate == stockHistory.HistoryDate) == 0)
                                                stockquotes.StockHistories.Add(stockHistory);
                                            stockquotes.SaveChanges();
                                            errorcnt = 0;
                                        }
                                    }
                                    catch (WebException ex)
                                    {
                                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                                        {
                                            var resp = (HttpWebResponse)ex.Response;
                                            if (resp.StatusCode == HttpStatusCode.NotFound)
                                            {
                                                errorcnt++;
                                                if (errorcnt > 7)
                                                    i = -3;
                                            }
                                        }
                                    }
                                }
                                i++;
                            }

                            break;
                        }
                }
                return;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [ActionName("SyncQuotes")]
        public HttpStatusCode SyncQuotes(int id)
        {
            OPIMsysContext stockquotes = new OPIMsysContext();
            var stocks = (from c in stockquotes.StockSymbols
                          select c.StockSymbolId).ToArray();

            foreach (int ids in stocks)
            {
                SyncQuote(ids);
            }
            return HttpStatusCode.OK;
        }

        private void SyncQuote(int id)
        {
            OPIMsysContext stockquotes = new OPIMsysContext();
            DateTime weekago = DateTime.Today.AddDays(-7).ToUniversalTime();
            DateTime lessFifteen = DateTime.Today.AddMinutes(-15).ToUniversalTime();
            //Remove any quotes over a week old or under 15 minutes
            var deleteQuotes = from r in stockquotes.StockQuotes
                               where (r.QuoteDate < weekago || r.QuoteDate > lessFifteen) && r.StockSymbolId == id
                               select r;
            foreach (var deleteQuote in deleteQuotes)
            {
                stockquotes.StockQuotes.Remove(deleteQuote);
            }

            var stock = stockquotes.StockSymbols.FirstOrDefault((s) => s.StockSymbolId == id);
            if (stock != null)
            {
                var market = (from s in stockquotes.Markets
                                 where s.MarketId == stock.MarketId
                                 select s).FirstOrDefault();

                switch (market.MarketName)
                {
                    case "NYM":
                        {
                            //Test for weekday
                            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                                return;

                            //Test for open hours
                            var time = market.MarketOpen.Split(':');
                            DateTime mrkOpen = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));
                            time = market.MarketClose.Split(':');
                            DateTime mrkClose = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));

                            if (DateTime.Now.ToUniversalTime() < mrkOpen || DateTime.Now.ToUniversalTime() > mrkClose)
                                return;
                            string symbol = stock.Symbol;
                            if (symbol == "CL")
                            {
                                switch (DateTime.Today.Month)
                                {
                                    case 1: symbol = "CLG"; break;
                                    case 2: symbol = "CLH"; break;
                                    case 3: symbol = "CLJ"; break;
                                    case 4: symbol = "CLK"; break;
                                    case 5: symbol = "CLM"; break;
                                    case 6: symbol = "CLN"; break;
                                    case 7: symbol = "CLQ"; break;
                                    case 8: symbol = "CLU"; break;
                                    case 9: symbol = "CLV"; break;
                                    case 10: symbol = "CLX"; break;
                                    case 11: symbol = "CLZ"; break;
                                    case 12: symbol = "CLF"; break;
                                }
                                symbol += DateTime.Today.Year.ToString().Substring(2);
                            }
                                    
                            string url = "http://finance.yahoo.com/d/quotes.csv?s=" + symbol + ".NYM" + "&f=d2ohgc1vl1";
                            WebRequest request = WebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            Stream dataStream = response.GetResponseStream();
                            StreamReader sr = new StreamReader(dataStream);
                            string line = sr.ReadLine();
                            List<string> returnVal = line.Split(',').ToList();
                            StockQuote quote = new StockQuote();
                            quote.StockSymbolId = stock.StockSymbolId;
                            quote.QuoteDate = DateTime.Now.ToUniversalTime();
                            if (returnVal[0] != "-")
                                quote.TradeDate = DateTime.Parse(returnVal[0]);
                            if (returnVal[1] != "N/A")
                                quote.Open = Decimal.Parse(returnVal[1]);
                            if (returnVal[2] != "N/A")
                                quote.High = Decimal.Parse(returnVal[2]);
                            if (returnVal[3] != "N/A")
                                quote.Low = Decimal.Parse(returnVal[3]);
                            if (returnVal[4] != "N/A")
                                quote.Change = Decimal.Parse(returnVal[4]);
                            if (returnVal[5] != "N/A")
                                quote.Volume = Int32.Parse(returnVal[5]);
                            if (returnVal[6] != "N/A")
                                quote.LastPrice = Decimal.Parse(returnVal[6]);
                            stockquotes.StockQuotes.Add(quote);
                            stockquotes.SaveChanges();
                            break;
                        }
                    case "NYSE":
                        {
                            //Test for weekday
                            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                                return;

                            //Test for open hours
                            var time = market.MarketOpen.Split(':');
                            DateTime mrkOpen = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));
                            time = market.MarketClose.Split(':');
                            DateTime mrkClose = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));

                            if (DateTime.Now.ToUniversalTime() < mrkOpen || DateTime.Now.ToUniversalTime() > mrkClose)
                                return;

                            //Get stock quote
                            string url = "http://finance.yahoo.com/d/quotes.csv?s=" + stock.Symbol + "" + "&f=d2ohgc1vj3eyl1";
                            WebRequest request = WebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            Stream dataStream = response.GetResponseStream();
                            StreamReader sr = new StreamReader(dataStream);
                            string line = sr.ReadLine();
                            List<string> returnVal = line.Split(',').ToList();
                            StockQuote quote = new StockQuote();
                            quote.StockSymbolId = stock.StockSymbolId;
                            quote.QuoteDate = DateTime.Now.ToUniversalTime();
                            if (returnVal[0] != "-")
                                quote.TradeDate = DateTime.Parse(returnVal[0]);
                            if (returnVal[1] != "N/A")
                                quote.Open = Decimal.Parse(returnVal[1]);
                            if (returnVal[2] != "N/A")
                                quote.High = Decimal.Parse(returnVal[2]);
                            if (returnVal[3] != "N/A")
                                quote.Low = Decimal.Parse(returnVal[3]);
                            if (returnVal[4] != "N/A")
                                quote.Change = Decimal.Parse(returnVal[4]);
                            if (returnVal[5] != "N/A")
                                quote.Volume = Int32.Parse(returnVal[5]);
                            if (returnVal[6] != "N/A")
                                quote.MarketCap = returnVal[6];
                            if (returnVal[7] != "N/A")
                                quote.EarningPerShare = Decimal.Parse(returnVal[7]);
                            if (returnVal[8] != "N/A")
                                quote.Dividend = Decimal.Parse(returnVal[8]);
                            if (returnVal[9] != "N/A")
                                quote.LastPrice = Decimal.Parse(returnVal[9]);

                            stockquotes.StockQuotes.Add(quote);
                            stockquotes.SaveChanges();
                            break;
                        }
                    case "TSX":
                        {
                            //Test for weekday
                            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                                return;

                            //Test for open hours
                            var time = market.MarketOpen.Split(':');
                            DateTime mrkOpen = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));
                            time = market.MarketClose.Split(':');
                            DateTime mrkClose = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));

                            if (DateTime.Now.ToUniversalTime() < mrkOpen || DateTime.Now.ToUniversalTime() > mrkClose)
                                return;

                            //Get stock quote
                            string url = "http://finance.yahoo.com/d/quotes.csv?s=" + stock.Symbol + ".TO" + "&f=d2ohgc1vj1eyl1";
                            WebRequest request = WebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            Stream dataStream = response.GetResponseStream();
                            StreamReader sr = new StreamReader(dataStream);
                            string line = sr.ReadLine();
                            List<string> returnVal = line.Split(',').ToList();
                            StockQuote quote = new StockQuote();
                            quote.StockSymbolId = stock.StockSymbolId;
                            quote.QuoteDate = DateTime.Now.ToUniversalTime();
                            if (returnVal[0] != "-")
                                quote.TradeDate = DateTime.Parse(returnVal[0]);
                            if (returnVal[1] != "N/A")
                                quote.Open = Decimal.Parse(returnVal[1]);
                            if (returnVal[2] != "N/A")
                                quote.High = Decimal.Parse(returnVal[2]);
                            if (returnVal[3] != "N/A")
                                quote.Low = Decimal.Parse(returnVal[3]);
                            if (returnVal[4] != "N/A")
                                quote.Change = Decimal.Parse(returnVal[4]);
                            if (returnVal[5] != "N/A")
                                quote.Volume = Int32.Parse(returnVal[5]);
                            if (returnVal[6] != "N/A")
                                quote.MarketCap = returnVal[6];
                            if (returnVal[7] != "N/A")
                                quote.EarningPerShare = Decimal.Parse(returnVal[7]);
                            if (returnVal[8] != "N/A")
                                quote.Dividend = Decimal.Parse(returnVal[8]);
                            if (returnVal[9] != "N/A")
                                quote.LastPrice = Decimal.Parse(returnVal[9]);

                            stockquotes.StockQuotes.Add(quote);
                            stockquotes.SaveChanges();
                            break;
                        }
                    case "TSX.V":
                        {
                            //Test for weekday
                            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                                return;

                            //Test for open hours
                            var time = market.MarketOpen.Split(':');
                            DateTime mrkOpen = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));
                            time = market.MarketClose.Split(':');
                            DateTime mrkClose = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));

                            if (DateTime.Now.ToUniversalTime() < mrkOpen || DateTime.Now.ToUniversalTime() > mrkClose)
                                return;

                            //Get stock quote
                            string url = "http://finance.yahoo.com/d/quotes.csv?s=" + stock.Symbol + ".V" + "&f=d2ohgc1vj1eyl1";
                            WebRequest request = WebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            Stream dataStream = response.GetResponseStream();
                            StreamReader sr = new StreamReader(dataStream);
                            string line = sr.ReadLine();
                            List<string> returnVal = line.Split(',').ToList();
                            StockQuote quote = new StockQuote();
                            quote.StockSymbolId = stock.StockSymbolId;
                            quote.QuoteDate = DateTime.Now.ToUniversalTime();
                            if (returnVal[0] != "-")
                                quote.TradeDate = DateTime.Parse(returnVal[0]);
                            if (returnVal[1] != "N/A")
                                quote.Open = Decimal.Parse(returnVal[1]);
                            if (returnVal[2] != "N/A")
                                quote.High = Decimal.Parse(returnVal[2]);
                            if (returnVal[3] != "N/A")
                                quote.Low = Decimal.Parse(returnVal[3]);
                            if (returnVal[4] != "N/A")
                                quote.Change = Decimal.Parse(returnVal[4]);
                            if (returnVal[5] != "N/A")
                                quote.Volume = Int32.Parse(returnVal[5]);
                            if (returnVal[6] != "N/A")
                                quote.MarketCap = returnVal[6];
                            if (returnVal[7] != "N/A")
                                quote.EarningPerShare = Decimal.Parse(returnVal[7]);
                            if (returnVal[8] != "N/A")
                                quote.Dividend = Decimal.Parse(returnVal[8]);
                            if (returnVal[9] != "N/A")
                                quote.LastPrice = Decimal.Parse(returnVal[9]);

                            stockquotes.StockQuotes.Add(quote);
                            stockquotes.SaveChanges();
                            break;
                        }
                }

            }
            else 
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}