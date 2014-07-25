using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using OPIMsys;


namespace OPIMsys.Controllers.Apis
{
    public class CompanyController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // GET api/company/5
        [ValidateInput(false)]
        public CompanyDTO Get(int id, int revision=0, string culture="en", string apikey="")
        {
            if (User.Identity.Name != null)
                if (!OPIMsys.Filters.ApiKeyHandler.ApiKeyToUser(apikey, Request))
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if (!Roles.IsUserInRole("ReportAPI") && !Roles.IsUserInRole("ApiReadUser"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            AccountApiKey apiUser = OPIMsys.Filters.ApiKeyHandler.KeyToAccount(apikey, Request);
            if(!Roles.IsUserInRole("ReportAPI"))
                if(id != apiUser.CompanyId)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            Language lang = db.Language.Where(a => a.Culture == culture).Single();
            
            Company comp = db.Companies.Find(id);
            if (comp.Revision > revision)
            {
                List<CompanyDTOSite> links = new List<CompanyDTOSite>();

                foreach (CompanyLink link in comp.CompanyLinks)
                {
                    if(link.LanguageId == lang.LanguageId)
                        links.Add(new CompanyDTOSite { Link = link.Link, Type = link.CompanyLinkType.Title });
                }
                List<CompanyDTOPage> pages = new List<CompanyDTOPage>();
                var enabledPages = comp.CompanyPages.Where(b => b.Enabled);
                if (enabledPages.Count() > 0)
                {
                    foreach (var ePages in enabledPages.GroupBy(a => a.PageId))
                    {
                        CompanyPage page = ePages.OrderByDescending(a => a.Revision).First();                        
                        pages.Add(new CompanyDTOPage { Content = page.Content, PubDate = page.PublishDate, Title = page.Title, PageName = page.PageName });
                    }
                }
                List<CompanyDTODocuments> docs = new List<CompanyDTODocuments>();
                String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                //String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                foreach (DocumentType doctype in db.DocumentTypes)
                {
                    List<CompanyDTODocument> tempDoc = new List<CompanyDTODocument>();
                    foreach (Document doc in comp.Documents.Where(a => a.DocumentTypeId == doctype.DocumentTypeId).Where(a => a.LanguageId == lang.LanguageId).OrderByDescending(a => a.PubDate))
                    {
                        tempDoc.Add(new CompanyDTODocument { Link = doc.Link, Title = doc.Title, ThumbnailLink = doc.ThumbnailLink, PubDate = doc.PubDate });
 
                    }
                    docs.Add(new CompanyDTODocuments { Name = doctype.Title, Documents = tempDoc.ToArray() });
                }

                

                List<CompanyDTOEvent> events = new List<CompanyDTOEvent>();
                foreach (Event evnt in comp.Events.OrderByDescending(a => a.StartTime))
                {
                    EventDetail evntDetail = (from a in evnt.EventDetails
                                            where a.LanguageId == lang.LanguageId
                                           select a).Single();
                    events.Add(new CompanyDTOEvent { 
                        StartTime = evnt.StartTime, 
                        EndTime = evnt.EndTime, 
                        Summary = evntDetail.Summary, 
                        Description = evntDetail.Description, 
                        Category = evnt.EventCategory.Title });
                }

                List<CompanyDTOStock> stocks = new List<CompanyDTOStock>();
                foreach (StockSymbol stock in comp.StockSymbols)
                {
                    List<CompanyDTODividend> dividends = new List<CompanyDTODividend>();
                    var tempDividends = (from a in db.StockDividends
                                         where a.StockSymbolId == stock.StockSymbolId
                                         select a).ToList();
                    foreach (var tempDividend in tempDividends)
                        dividends.Add(new CompanyDTODividend { 
                                Dividend = tempDividend.Dividend, 
                                ExDividendDate = tempDividend.ExDividendDate, 
                                Notes = tempDividend.Notes, 
                                PayableDate = tempDividend.PayableDate, 
                                RecordDate = tempDividend.RecordDate });


                    stocks.Add(new CompanyDTOStock { Market = stock.Market.MarketName, Symbol = stock.Symbol, Dividends=dividends.ToArray() });
                }
                List<CompanyDTOPeople> people = new List<CompanyDTOPeople>();
                foreach (PeopleType peopleType in db.PeopleTypes)
                {
                    List<CompanyDTOPerson> persons = new List<CompanyDTOPerson>();
                    var tempPeople = from a in comp.People
                                        from b in a.PeopleTypes
                                        where b.PeopleTypeId.Equals(peopleType.PeopleTypeId)
                                     select a;
                    foreach(People person in tempPeople)
                    {
                        var info = person.PeopleInformation.Where(a => a.LanguageId == lang.LanguageId);

                        if (info.Count() > 0)
                        {
                            var sinfo = info.Single();
                            persons.Add(new CompanyDTOPerson
                            {
                                Address = person.Address,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Email = person.Email,
                                Phone = person.Phone,
                                Company = person.CompanyName,
                                Bio = sinfo.Bio,
                                Title = sinfo.Title,
                                ImageURL = person.ImageURL
                            });
                        }
                        else
                        {
                            persons.Add(new CompanyDTOPerson
                            {
                                Address = person.Address,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Email = person.Email,
                                Phone = person.Phone,
                                Company = person.CompanyName,
                                Bio = "",
                                Title = "",
                                ImageURL = person.ImageURL
                            });
                        }

                    }
              
                    people.Add(new CompanyDTOPeople { Name = peopleType.Title, Persons = persons.ToArray() });
                }
                
                return new CompanyDTO { 
                    Name = comp.Name, 
                    Revision = comp.Revision, 
                    ShortDescription = comp.CompanyInformation.Where(a => a.LanguageId == lang.LanguageId).Single().ShortDescription,
                    LongDescription = comp.CompanyInformation.Where(a => a.LanguageId == lang.LanguageId).Single().LongDescription,
                    Letter = comp.CompanyInformation.Where(a => a.LanguageId == lang.LanguageId).Single().Letter,
                    Slogan = comp.CompanyInformation.Where(a => a.LanguageId == lang.LanguageId).Single().Slogan,
                    Contact = comp.CompanyInformation.Where(a => a.LanguageId == lang.LanguageId).Single().Contact,
                    Strategy = comp.CompanyInformation.Where(a => a.LanguageId == lang.LanguageId).Single().Strategy,
                    LogoURL = comp.LogoURL,
                    Links=links.ToArray(),
                    Stocks = stocks.ToArray(),
                    People = people.ToArray(),
                    Events = events.ToArray(),
                    Documents = docs.ToArray(),
                    Pages = pages.ToArray()
                };

            }
            else
                throw new HttpResponseException(HttpStatusCode.NoContent); 
       
        }

        // POST api/company
        
        public void Post([FromBody]string value)
        {
        }


        // PUT api/company/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/company/5
        public void Delete(int id)
        {
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public bool IsFollowerRegistered(int id, string emailAddress, string apikey = "")
        {
            if (User.Identity.Name != null)
                if (!OPIMsys.Filters.ApiKeyHandler.ApiKeyToUser(apikey, Request))
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if (!Roles.IsUserInRole("ReportAPI") && !Roles.IsUserInRole("ApiReadUser"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            AccountApiKey apiUser = OPIMsys.Filters.ApiKeyHandler.KeyToAccount(apikey, Request);
            if (!Roles.IsUserInRole("ReportAPI"))
                if (id != apiUser.CompanyId)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));

            Company comp = db.Companies.Find(id);
            if (comp == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            if (comp.CompanyFollowers.Where(a => a.EmailAddress == emailAddress).Count() > 0)
                return true;
            return false;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public bool FollowerRegister(int id, string emailAddress, string firstName, string lastName, string company = "", string apikey = "")
        {
            if (User.Identity.Name != null)
                if (!OPIMsys.Filters.ApiKeyHandler.ApiKeyToUser(apikey, Request))
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            if (!Roles.IsUserInRole("ReportAPI") && !Roles.IsUserInRole("ApiReadUser"))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            AccountApiKey apiUser = OPIMsys.Filters.ApiKeyHandler.KeyToAccount(apikey, Request);
            if (!Roles.IsUserInRole("ReportAPI"))
                if (id != apiUser.CompanyId)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));

            OPIMsys.RegexUtilities regUtil = new RegexUtilities();
            if (!regUtil.IsValidEmail(emailAddress))
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Conflict));
            if(db.CompanyFollowers.Where(a => a.EmailAddress == emailAddress).Count() > 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Conflict));
            Company comp = db.Companies.Find(id);
            if (comp == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            CompanyFollower follow = new CompanyFollower();
            follow.EmailAddress = emailAddress;
            follow.FirstName = firstName;
            follow.LastName = lastName;
            follow.CompanyName = company;
            comp.CompanyFollowers.Add(follow);
            db.SaveChanges();
            return true;
        }

        
        [System.Web.Mvc.Authorize]
        public string Info()
        {
            return User.Identity.Name;
        }
    }
}
