using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace OPIMsys.Controllers.Apis
{
    public class DividendController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // PUT api/News/5
        public HttpResponseMessage PutDividend(StockDividendApi newapi)
        {
            AccountApiKey apiUser = GetAccount();
            StockDividend dividend = new StockDividend();
            try
            {
                StockSymbol symbol = db.StockSymbols
                                    .Where(a => a.CompanyId == apiUser.CompanyId)
                                    .Where(a => a.Symbol == newapi.Symbol)
                                    .Where(a => a.Market.MarketName == newapi.Market)
                                    .Single();
                dividend = db.StockDividends.Where(a => a.StockSymbolId == symbol.StockSymbolId).Where(a => a.RecordDate == newapi.RecordDate).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            dividend.Dividend = newapi.Dividend;
            dividend.ExDividendDate = newapi.ExDividendDate;
            dividend.Notes = newapi.Notes;
            dividend.PayableDate = newapi.PayableDate;
            dividend.RecordDate = newapi.RecordDate;

            if (ModelState.IsValid)
            {
                //db.Entry(news).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/News
        public HttpResponseMessage PostDividend(StockDividendApi newapi)
        {
            AccountApiKey apiUser = GetAccount();
            StockDividend dividend = new StockDividend();
            StockSymbol symbol = new StockSymbol();
            try
            {
                symbol = db.StockSymbols
                                    .Where(a => a.CompanyId == apiUser.CompanyId)
                                    .Where(a => a.Symbol == newapi.Symbol)
                                    .Where(a => a.Market.MarketName == newapi.Market)
                                    .Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            dividend = new StockDividend
            {
                Dividend = newapi.Dividend,
                ExDividendDate = newapi.ExDividendDate,
                Notes = newapi.Notes,
                PayableDate = newapi.PayableDate,
                RecordDate = newapi.RecordDate,
                StockSymbolId = symbol.StockSymbolId
            };
            if (ModelState.IsValid)
            {
                db.StockDividends.Add(dividend);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/News/5
        public HttpResponseMessage DeleteDividend(string market, string symbol, DateTime recordDate)
        {
            AccountApiKey apiUser = GetAccount();
            StockDividend dividend = new StockDividend();
            try
            {
                StockSymbol sSymbol = db.StockSymbols
                                    .Where(a => a.CompanyId == apiUser.CompanyId)
                                    .Where(a => a.Symbol == symbol)
                                    .Where(a => a.Market.MarketName == market)
                                    .Single();
                dividend = db.StockDividends.Where(a => a.StockSymbolId == sSymbol.StockSymbolId).Where(a => a.RecordDate == recordDate).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            db.StockDividends.Remove(dividend);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private AccountApiKey GetAccount()
        {
            IEnumerable<string> apiKeyHeaderValues = null;
            if (Request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();
                var webapis = db.AccountApiKeys.Where(a => a.ApiKey == apiKeyHeaderValue);
                if (webapis.Count() == 1)
                    return webapis.Single();
            }
            throw new Exception("No Company associated with that API key");
        }

    }
}
