using Newtonsoft.Json;
using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace OPIMsys.Controllers.Apis
{
    public class NewsController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // GET api/News/sync
        [HttpGet]
        [ActionName("SyncNews")]
        public HttpResponseMessage SyncNews(int id=0)
        {
            //Get all Companies with sources
            var companies = from a in db.NewsSources select a;
            foreach (var companySource in companies)
            {
                switch (companySource.NewsSourceType.Title)
                {
                    case "GoogleMarketWire":
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load("http://" + companySource.Link);

                            XmlNodeList list = doc.SelectNodes("//item");

                            int newsTypeId = (from a in db.NewsType where a.Title == "Press Release" select a.NewsTypeId).Single();

                            foreach (XmlNode item in list)
                            {
                                string newsId = item.SelectSingleNode("guid").InnerXml;
                                if (item.SelectSingleNode("description").InnerXml.Contains("MarketWatch (press release)"))
                                {
                                    if (db.News.Where(a => a.NewsSourceIdentifier == newsId).Where(a => a.NewsSourceId == companySource.NewsSourceId).Count() == 0)
                                    {
                                        News news = new News();
                                        news.NewsSourceIdentifier = newsId;
                                        news.PubDate = DateTime.Parse(item.SelectSingleNode("pubDate").InnerXml);
                                        //news.PDFLink = item.SelectSingleNode("pdf_url").InnerXml;
                                        news.Title = item.SelectSingleNode("title").InnerText.Trim();
                                        news.Link = item.SelectSingleNode("link").InnerText;
                                        news.NewsSourceId = companySource.NewsSourceId;
                                        news.NewsTypeId = newsTypeId;
                                        news.Description = item.SelectSingleNode("description").InnerText;
                                        news.Content = "";
                                        db.News.Add(news);
                                        Mobile alert = new Mobile { companyId = companySource.CompanyId, text = news.Title, type = "News" };
                                        SendNotifications(alert);
                                    }
                                }
                            }

                            break;
                        }
                    case "MarketWiredImpress":
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load("http://" + companySource.Link);

                            XmlNodeList list = doc.SelectNodes("//item");

                            int newsTypeId = (from a in db.NewsType where a.Title == "Press Release" select a.NewsTypeId).Single();

                            foreach (XmlNode item in list)
                            {
                                string newsId = item.SelectSingleNode("id").InnerXml;
                                if (db.News.Where(a => a.NewsSourceIdentifier == newsId).Where(a => a.NewsSourceId == companySource.NewsSourceId).Count() == 0)
                                {
                                    News news = new News();
                                    news.NewsSourceIdentifier = newsId;
                                    news.PubDate = DateTime.Parse(item.SelectSingleNode("date").InnerXml);
                                    news.PDFLink = item.SelectSingleNode("pdf_url").InnerXml;
                                    news.Title = item.SelectSingleNode("title").InnerText.Trim();
                                    if (item.SelectSingleNode("print").InnerText != "")
                                        news.Link = item.SelectSingleNode("print").InnerText;
                                    else if (item.SelectSingleNode("link").InnerText != "")
                                        news.Link = item.SelectSingleNode("link").InnerText;
                                    else
                                        news.Link = "NA";
                                    news.NewsSourceId = companySource.NewsSourceId;
                                    news.NewsTypeId = newsTypeId;
                                    news.Description = "";
                                    news.Content = "";
                                    db.News.Add(news);
                                    Mobile alert = new Mobile { companyId = companySource.CompanyId, text = news.Title, type = "News" };
                                    SendNotifications(alert);
                                }
                            }

                            break;
                        }
                    case "MarketWireOilGas":
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load("http://" + companySource.Link);
                            
                            XmlNodeList list = doc.SelectNodes("//item");

                            int newsTypeId = (from a in db.NewsType where a.Title == "Press Release" select a.NewsTypeId).Single();

                            foreach (XmlNode item in list)
                            {
                                string newsId = item.SelectSingleNode("id").InnerXml;
                                if (db.News.Where(a => a.NewsSourceIdentifier == newsId).Where(a =>a.NewsSourceId == companySource.NewsSourceId).Count() == 0)
                                {
                                    News news = new News();
                                    news.NewsSourceIdentifier = newsId;
                                    news.PubDate = DateTime.Parse(item.SelectSingleNode("date").InnerXml);
                                    news.PDFLink = item.SelectSingleNode("pdf_url").InnerXml;
                                    news.Title = item.SelectSingleNode("title").InnerText.Trim();
                                    if (item.SelectSingleNode("print").InnerText != "")
                                        news.Link = item.SelectSingleNode("print").InnerText;
                                    else if (item.SelectSingleNode("link").InnerText != "")
                                        news.Link = item.SelectSingleNode("link").InnerText;
                                    else
                                        news.Link = "NA";
                                    news.NewsSourceId = companySource.NewsSourceId;
                                    news.NewsTypeId = newsTypeId;
                                    news.Description = "";
                                    news.Content = "";
                                    db.News.Add(news);
                                    Mobile alert = new Mobile { companyId = companySource.CompanyId, text = news.Title, type = "News" };
                                    SendNotifications(alert);
                                }
                            }
                           
                            break;
                        }
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }       
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        private void SendNotifications(Mobile alert)
        {
            HttpClient client;
            string key = "dxnPCfwbAHhxTwNXggXLRsQXLljdBr53";
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", key);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var obj = JsonConvert.SerializeObject(alert, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var request = new HttpRequestMessage(HttpMethod.Post, "https://opimsys.azure-mobile.net/tables/updates");
            request.Content = new StringContent(obj, Encoding.UTF8, "application/json");

            var data = client.SendAsync(request).Result;

            if (!data.IsSuccessStatusCode)
                throw new Exception(data.StatusCode.ToString());
        }

        [HttpGet]
        [ActionName("AlertNews")]
        public HttpResponseMessage AlertNews(string text = "")
        {
         
            Mobile alert = new Mobile { text = text, type = "News Alert", companyId=4 };

            try
            {
                SendNotifications(alert);
            }
            catch (Exception err)
            {
                throw new HttpResponseException(HttpStatusCode.SeeOther);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
 /*       // GET api/News
        public IEnumerable<News> GetNews()
        {
            var news = db.News.Include(n => n.Language).Include(n => n.NewsType).Include(n => n.Company).Include(n => n.NewsSource);
            return news.AsEnumerable();
        }
*/
        // GET api/News/5
        public NewsDTO[] GetNews(int id, string culture="en")
        {
            Company company = db.Companies.Find(id);
            //News news = db.News.Find(id);
            if (company == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            var lang = db.Language.Where(a => a.Culture == culture).Single();
            var newSources = (from a in db.NewsSources
                              where a.LanguageId == lang.LanguageId
                              where a.CompanyId == company.CompanyId
                              select a.NewsSourceId).ToList();
            var news = (from b in db.News
                        where newSources.Contains(b.NewsSourceId)
                        orderby b.PubDate descending
                        select b).ToList();

            List<NewsDTO> retList = news.ConvertAll(x =>
                new NewsDTO { Title = x.Title, Description = x.Description, Content = x.Content, Link = x.Link, PDFLink = x.PDFLink, PubDate = x.PubDate });
            
            return retList.ToArray();
        }

        // PUT api/News/5
        public HttpResponseMessage PutNews(string id, NewsDTO newapi)
        {
            AccountApiKey apiUser = GetAccount();
            NewsSourceType newsType = db.NewsSourceTypes.Where(a => a.Title == "API").Single();
            NewsSource newsSource = db.NewsSources.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.NewsSourceTypeId == newsType.NewsSourceTypeId).Single();
            News news = new News();
            try
            {
                news = db.News.Where(a => a.NewsSourceId == newsSource.NewsSourceId).Where(a => a.NewsSourceIdentifier == id).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            news.Content = newapi.Content;
            news.Description = newapi.Description;
            news.Link = newapi.Link;
            news.PDFLink = newapi.PDFLink;
            news.PubDate = newapi.PubDate;
            news.Title = newapi.Title;
            news.NewsTypeId = db.NewsType.Where(a => a.Title == "Press Release").Single().NewsTypeId;
            news.NewsSourceIdentifier = id;
            news.NewsSourceId = newsSource.NewsSourceId;
            
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
        public HttpResponseMessage PostNews(NewsDTOAPI newapi)
        {
            AccountApiKey apiUser = GetAccount();
            NewsSourceType newsType = db.NewsSourceTypes.Where(a => a.Title == "API").Single();
            NewsSource newsSource = db.NewsSources.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.NewsSourceTypeId == newsType.NewsSourceTypeId).Single();
            var newsList = db.News.Where(a => a.NewsSourceId == newsSource.NewsSourceId).Where(a => a.NewsSourceIdentifier == newapi.Id);
            if(newsList.Count() > 0)
                return Request.CreateResponse(HttpStatusCode.Found);

            News news = new News
            {
                Content = newapi.Content,
                Description = newapi.Description,
                Link = newapi.Link,
                PDFLink = newapi.PDFLink,
                PubDate = newapi.PubDate,
                Title = newapi.Title,
                NewsTypeId = db.NewsType.Where(a => a.Title == "Press Release").Single().NewsTypeId,
                NewsSourceIdentifier = newapi.Id,
                NewsSourceId = newsSource.NewsSourceId
            };
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/News/5
        public HttpResponseMessage DeleteNews(string id)
        {
            AccountApiKey apiUser = GetAccount();
            NewsSourceType newsType = db.NewsSourceTypes.Where(a => a.Title == "API").Single();
            NewsSource newsSource = db.NewsSources.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.NewsSourceTypeId == newsType.NewsSourceTypeId).Single();
            News news = db.News.Where(a => a.NewsSourceId == newsSource.NewsSourceId).Where(a => a.NewsSourceIdentifier == id).Single();
            if (news == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.News.Remove(news);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, news);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private AccountApiKey GetAccount()
        {
            IEnumerable<string> apiKeyHeaderValues = null;
            if(Request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();
                var webapis = db.AccountApiKeys.Where(a => a.ApiKey == apiKeyHeaderValue);
                if(webapis.Count() == 1)
                    return webapis.Single();
            }
            throw new Exception("No Company associated with that API key");
        }


     }
}