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
using WebMatrix.WebData;
using System.Web.Security;

namespace OPIMsys.Controllers.Apis
{
    public class TableController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        public CompanyDTOMarketComparison[] GetComparison(int id = 0, bool peers = false, bool markets = false, int maxmonths=60, string apikey = "")
        {
            int companyId = 0;
            if (User.Identity.Name != null)
                if (!OPIMsys.Filters.ApiKeyHandler.ApiKeyToUser(apikey, Request))
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if (!Roles.IsUserInRole("ReportAPI") && !Roles.IsUserInRole("ApiReadUser"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            AccountApiKey apiUser = OPIMsys.Filters.ApiKeyHandler.KeyToAccount(apikey, Request);
            if (apiUser == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "No user found"));
            if (apiUser.CompanyId == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "No company found"));
            companyId = apiUser.CompanyId;
            
            List<CompanyDTOMarketComparison> marketComps = new List<CompanyDTOMarketComparison>();
            //int[] months = { 1, 3, 6, 12, 24, 60 };
            int month = maxmonths;
           // List<int> baseMonths = new List<int>(months) ;
           // foreach (int i in baseMonths.FindAll(a => a.CompareTo(maxmonths) > 0))
           //     baseMonths.Remove(i);
           // months = baseMonths.ToArray();
            
            List<int> ids = new List<int>();
            if (markets)
            {
                foreach (int sId in db.CompanyMarketComparisons.Where(a => a.CompanyId == companyId).Select(a => a.StockSymbolId))
                {
                    ids.Add(sId);
                }
            }
            if (peers)
            {
                foreach (int cId in db.CompanyPeers.Where(a => a.CompanyId == companyId).Select(a => a.PeerId))
                {
                    foreach(int sId in db.StockSymbols.Where(a => a.CompanyId == cId).Select(a => a.StockSymbolId))
                        ids.Add(sId);
                }
            }
            foreach (int sId in db.StockSymbols.Where(a => a.CompanyId == companyId).Select(a => a.StockSymbolId))
            {
                ids.Add(sId);
            }
            var comps = db.StockSymbols.Where(a => ids.Contains(a.StockSymbolId));
        //    foreach (int month in months)
        //    {
                CompanyDTOMarketComparison marketComp = new CompanyDTOMarketComparison();
                marketComp.Months = month;
                List<CompanyDTOMarketComparisonStock> compStocks = new List<CompanyDTOMarketComparisonStock>();
                int day = 1;
                if (month == 1) day = 1;
                else if (month <= 6) day = 1;
                else if (month <= 12) day = 4;
                else if (month <= 36) day = 5;
                else { day = 6; }
                foreach (var sComp in comps)
                {
                    CompanyDTOMarketComparisonStock compStock = new CompanyDTOMarketComparisonStock();
                    compStock.StockSymbolId = sComp.StockSymbolId;
                    compStock.Market = sComp.Market.MarketName;
                    compStock.Symbol = sComp.Symbol;
                    DateTime startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var history = db.StockHistories.Where(a => a.StockSymbolId == compStock.StockSymbolId).Where(b => b.HistoryDate > startdate);
                    startdate = startdate.AddMonths(-1 * month);
                    DateTime curdate = startdate;
                    List<CompanyDTOMarketComparisonData> compDatas = new List<CompanyDTOMarketComparisonData>();
                    CompanyDTOMarketComparisonData compData = new CompanyDTOMarketComparisonData();
                    compData.date = curdate;
                    compData.value = 0;
                    compDatas.Add(compData);
                    curdate = curdate.AddDays(day);
                    decimal openVal = 0;
                    
                    while (curdate < DateTime.Now)
                    {
                        var curHistory = history.Where(b => b.HistoryDate == curdate);
                        if (curHistory != null && curHistory.Count() > 0)
                        {
                            CompanyDTOMarketComparisonData compData1 = new CompanyDTOMarketComparisonData();
                            compData1.date = curdate;
                            if (openVal == 0) openVal = history.First().Open;
                            decimal curVal = (curHistory.First().Close - openVal) / openVal;
                            compData1.value = decimal.Round(curVal * 100, 2);
                            compDatas.Add(compData1);
                        }
                        
                            curdate = curdate.AddDays(day);
                    }
                    compStock.Data = compDatas.ToArray();
                    compStocks.Add(compStock);
                }
                marketComp.Stocks = compStocks.ToArray();
                marketComps.Add(marketComp);

          //  }
            return marketComps.ToArray();
        }


        // GET api/Table
        
        public CompanyDataChartDTO GetData(int id = 0, bool comparison = false, int datasets = 1, int groupid =0, bool isreport=false, string apikey ="")
        {
            int companyId = 0;
            if (User.Identity.Name != null)
                if (!OPIMsys.Filters.ApiKeyHandler.ApiKeyToUser(apikey, Request))
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if (!Roles.IsUserInRole("ReportAPI") && !Roles.IsUserInRole("ApiReadUser"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            AccountApiKey apiUser = OPIMsys.Filters.ApiKeyHandler.KeyToAccount(apikey, Request);
            if(apiUser == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound,"No user found"));
            if (isreport)
            {
                //Validate IQ or reporting 
                if (!Roles.IsUserInRole("ReportAPI"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
                }
            }
            else
            {
                if(apiUser.CompanyId == null)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound,"No company found"));    
                companyId = apiUser.CompanyId;
            }
            var chart = db.CompanyDataTypes.Find(id);
            if(chart ==null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            List<CompanyDataDTO> companyData = new List<CompanyDataDTO>();
            if (comparison)
            {
                var companies = chart.Companies.OrderBy(a => a.Company.Name);
                //select the company ... find its peers if the comparison is set 
                if (companyId != 0)
                {
                    var peers = db.CompanyPeers.Where(a => a.CompanyId == companyId).Select(a => a.PeerId).ToList();
                    companies = companies.Where(a => peers.Contains(a.CompanyId) || a.CompanyId == companyId).OrderBy(a => a.Company.Name);
                }
                else if (isreport && groupid != 0)
                {
                    var compId = db.CompanyGroups.Where(a => a.GroupId == groupid).Select(a => a.CompanyId).ToList();
                    companies = companies.Where(a => compId.Contains(a.CompanyId)).OrderBy(a => a.Company.Name);
                }

                foreach (var company in companies)
                {
                    List<CompanyDataVaribleDTO> variables = new List<CompanyDataVaribleDTO>();
                    List<DateTime> dates = new List<DateTime>();
                    int ind = 0;
                    foreach (var variable in chart.Variables)
                    {
                        List<decimal> values = new List<decimal>();
                        if (datasets == 0)
                        {
                            var value = db.CompanyData.Where(a => a.CompanyId == company.CompanyId)
                                                       .Where(a => a.CompanyDataVariableId == variable.CompanyDataVariableId)
                                                       .OrderByDescending(a => a.publishDate)
                                                       .First();
                            if (ind == 0) dates.Add(value.publishDate);
                            values.Add(value.Value);

                        }
                        else
                        {
                            var value = db.CompanyData.Where(a => a.CompanyId == company.CompanyId)
                                                       .Where(a => a.CompanyDataVariableId == variable.CompanyDataVariableId)
                                                       .OrderByDescending(a => a.publishDate).ToArray();
                            if (value.Count() < datasets)
                                datasets = value.Count();
                            for (int j = 0; j < datasets; j++)
                            {
                                if (ind == 0) dates.Add(value[j].publishDate);
                                values.Add(value[j].Value);
                            }
                        }
                        variables.Add(new CompanyDataVaribleDTO { Values = values.ToArray(), VariableName = variable.Title });
                        ind++;
                    }
                    companyData.Add(new CompanyDataDTO { Company = company.Company.Name, Dates = dates.ToArray(), Variables = variables.ToArray() });
                }
            }
            else
            {
                if (companyId == 0 && groupid != 0)
                    companyId = groupid;
                Company company = db.Companies.Find(companyId);
                List<CompanyDataVaribleDTO> variables = new List<CompanyDataVaribleDTO>();
                List<DateTime> dates = new List<DateTime>();
                int ind = 0;
                foreach (var variable in chart.Variables)
                {
                    List<decimal> values = new List<decimal>();
                    if (datasets == 0)
                    {
                        var value = db.CompanyData.Where(a => a.CompanyId == company.CompanyId)
                                                   .Where(a => a.CompanyDataVariableId == variable.CompanyDataVariableId)
                                                   .OrderByDescending(a => a.publishDate)
                                                   .First();
                        if (ind == 0) dates.Add(value.publishDate);
                        values.Add(value.Value);

                    }
                    else
                    {
                        var value = db.CompanyData.Where(a => a.CompanyId == company.CompanyId)
                                                   .Where(a => a.CompanyDataVariableId == variable.CompanyDataVariableId)
                                                   .OrderByDescending(a => a.publishDate).ToArray();
                        if (value.Count() < datasets)
                            datasets = value.Count();
                        for (int j = 0; j < datasets; j++)
                        {
                            if (ind == 0) dates.Add(value[j].publishDate);
                            values.Add(value[j].Value);
                        }
                    }
                    variables.Add(new CompanyDataVaribleDTO { Values = values.ToArray(), VariableName = variable.Title });
                    ind++;
                }
                companyData.Add(new CompanyDataDTO { Company = company.Name, Dates = dates.ToArray(), Variables = variables.ToArray() });

            }
            return new CompanyDataChartDTO { Title = chart.Title, Notes = chart.Notes, Data = companyData.ToArray() };
        }
     

  /*      // GET api/Table/5
        public CompanyData GetCompanyData(int id)
        {
            CompanyData companydata = db.CompanyData.Find(id);
            if (companydata == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return companydata;
        }

        // PUT api/Table/5
        public HttpResponseMessage PutCompanyData(int id, CompanyData companydata)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != companydata.CompanyDataId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(companydata).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Table
        public HttpResponseMessage PostCompanyData(CompanyData companydata)
        {
            if (ModelState.IsValid)
            {
                db.CompanyData.Add(companydata);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, companydata);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = companydata.CompanyDataId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Table/5
        public HttpResponseMessage DeleteCompanyData(int id)
        {
            CompanyData companydata = db.CompanyData.Find(id);
            if (companydata == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CompanyData.Remove(companydata);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, companydata);
        }
*/
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

       
    }
}