using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;
using System.Windows.Documents;



namespace OPIMsys.Controllers
{
    public class CompanyController : Controller
    {
        private OPIMsysContext db = new OPIMsysContext();
        #region Company
        //
        // GET: /Company/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            
            return View(db.Companies.OrderBy(a => a.Name).ToList());
        }

        //
        // GET: /Company/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.Languages = db.Language.ToList();
            var langs = from c in db.Language
                where (from o in db.CompanyInformation
                            where o.CompanyId == id
                            select o.Language.Culture)
                            .Contains(c.Culture)
                select c;
            ViewBag.DocumentTypes = db.DocumentTypes.ToList();
            ViewBag.CompanyLanguages = langs.ToList();

            return View(company);
        }

        //
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                company.Revision++;
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        //
        // GET: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = company.CompanyId } );
            }
            return View(company);
        }

        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
        #region CompanyInfo
        
        //
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateCompanyInformation(int companyid = 0)
        {
            Company company = db.Companies.Find(companyid);
            if (company == null)
                throw new HttpException(404, "Company not found");
            CompanyInformation compinfo = new CompanyInformation();
            compinfo.CompanyId = companyid;
            var langs = from c in db.Language
                                where !(from o in db.CompanyInformation
                                            where o.CompanyId == companyid
                                            select o.Language.LanguageId)
                                            .Contains(c.LanguageId)
                                select c;

            ViewBag.LanguageList = new SelectList(langs.ToList(), "LanguageId", "Title");
            return View(compinfo);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreateCompanyInformation(CompanyInformation type)
        {
            type.LanguageId = Int16.Parse(this.Request.Form.Get("LanguageList"));
            
            if (ModelState.IsValid)
            {
                Company company =db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.CompanyInformation.Add(type);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=type.CompanyId });
            }

            return View(type);
        }

        //
        // GET: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult EditCompanyInformation(int id = 0)
        {
            //CompanyInformation type = db.CompanyInformation.Find(id);
            CompanyInformation type = db.CompanyInformation.Include(a => a.Language).Include(a => a.Company).Where(a => a.CompanyInformationId == id).Single();
            if (type == null)
            {
                return HttpNotFound();
            }
            return View("CreateCompanyInformation", type);
        }

        //
        // POST: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult EditCompanyInformation(CompanyInformation type)
        {
            
            if (ModelState.IsValid)
            {
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = company.CompanyId });
            }
            return View("CreateCompanyInformation", type);
        }
        #endregion
        #region CompanyPages
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateCompanyPage(int id = 0, int companyid = 0, int languageid=0)
        {
            

            CompanyPage page = new CompanyPage();
            if (id == 0)
            {
                Company company = db.Companies.Find(companyid);
                if (company == null)
                    HttpNotFound();
                Language language = db.Language.Find(languageid);
                if (language == null)
                    HttpNotFound();
                //Create the new page
                page.CompanyId = company.CompanyId;
                page.LanguageId = language.LanguageId;
                page.Enabled = false;
                page.PublishDate = DateTime.Now;
            }
            else
            {
                //Edit the page
                page = db.CompanyPages.Find(id);
                if (page == null)
                    HttpNotFound();
            }
            return View(page);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateCompanyPage(CompanyPage type)
        {
            Company company = db.Companies.Find(type.CompanyId);
            if (company == null)
                HttpNotFound();
            CompanyPage page = new CompanyPage();
            page = type;
            page.UserName = User.Identity.Name;
            //page.PublishDate = ;
            page.EditDate = DateTime.Now;
            if (type.CompanyPageId == 0)
            {
                //Create the new page
                var value = (from a in db.CompanyPages select a.PageId);
                if (value.Count() == 0)
                    page.PageId = 1;
                else
                    page.PageId = value.Max() + 1;
                page.Revision = 0;
            }
            else
            {
                page.Revision = (from a in db.CompanyPages where a.PageId == page.PageId select a.Revision).Max() + 1;
            }
            db.CompanyPages.Add(page);
            company.Revision++;
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ToggleCompanyPage(int id)
        {
            CompanyPage type = db.CompanyPages.Find(id);
            if (type == null)
                HttpNotFound();
            if (type.Enabled) type.Enabled = false;
            else type.Enabled = true;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCompanyPage(int id = 0)
        {
            CompanyPage type = db.CompanyPages.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteCompanyPage")]
        public ActionResult DeleteCompanyPageConfirmed(int id)
        {
            CompanyPage type = db.CompanyPages.Find(id);
            db.CompanyPages.Remove(type);
            Company company = db.Companies.Find(type.CompanyId);
            company.Revision++;
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        #endregion
        #region StockSymbols
        //
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateSymbol(int companyid = 0)
        {
            Company company = db.Companies.Find(companyid);
            if (company == null)
                HttpNotFound();
            StockSymbol type = new StockSymbol();
            type.CompanyId = companyid;
            ViewBag.MarketList = new SelectList(db.Markets.ToList(), "MarketId", "MarketName");
            return View(type);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreateSymbol(StockSymbol type)
        {
            type.MarketId = Int16.Parse(this.Request.Form.Get("MarketList"));
            if (ModelState.IsValid)
            {
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.StockSymbols.Add(type);
                db.SaveChanges();
                //To-Do add the sync for the individual stock ... 


                return RedirectToAction("Details", new { id = type.CompanyId });
            }

            return View(type);
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteSymbol(int id = 0)
        {
            StockSymbol type = db.StockSymbols.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteSymbol")]
        public ActionResult DeleteSymbolConfirmed(int id)
        {
            StockSymbol type = db.StockSymbols.Find(id);
            //Remove all the history
            var history = from a in db.StockHistories
                          where a.StockSymbolId == id
                          select a;
            foreach (var item in history)
                db.StockHistories.Remove(item);
            //Remove all the quotes
            var quotes = from a in db.StockQuotes
                         where a.StockSymbolId == id
                         select a;
            foreach (var item in quotes)
                db.StockQuotes.Remove(item);
            Company company = db.Companies.Find(type.CompanyId);
            company.Revision++;
            db.Entry(company).State = EntityState.Modified;
            db.StockSymbols.Remove(type);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        
        
        #endregion
        #region CompanyLinks

        //
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateCompanyLink(int companyid = 0)
        {
            Company company = db.Companies.Find(companyid);
            if (company == null)
                HttpNotFound();
            CompanyLink compinfo = new CompanyLink();
            compinfo.CompanyId = companyid;
            ViewBag.LinkTypeList = new SelectList(db.CompanyLinkType.ToList(), "CompanyLinkTypeId", "Title");
            ViewBag.LanguageList = new SelectList(db.Language.ToList(), "LanguageId", "Title");
            return View(compinfo);
        }
        
        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreateCompanyLink(CompanyLink type)
        {
            if (type.LanguageId == 0)
                type.LanguageId = Int16.Parse(this.Request.Form.Get("LanguageList"));
            
            if(type.CompanyLinkTypeId == 0)
                type.CompanyLinkTypeId = Int16.Parse(this.Request.Form.Get("LinkTypeList"));
            
            if (ModelState.IsValid)
            {
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.CompanyLink.Add(type);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = type.CompanyId });
            }
            ViewBag.LinkTypeList = new SelectList(db.CompanyLinkType.ToList(), "CompanyLinkTypeId", "Title");
            ViewBag.LanguageList = new SelectList(db.Language.ToList(), "Culture", "Title");
            return View(type);
        }

        //
        // GET: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult EditCompanyLink(int id = 0)
        {
            //CompanyInformation type = db.CompanyInformation.Find(id);
            CompanyLink type = db.CompanyLink.Include(a => a.Language).Include(a => a.Company).Where(a => a.CompanyLinkId == id).Single();
            if (type == null)
            {
                return HttpNotFound();
            }
            return View("CreateCompanyLink", type);
        }

        //
        // POST: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult EditCompanyLink(CompanyLink type)
        {
            if (ModelState.IsValid)
            {
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = type.CompanyId });
            }
            return View("CreateCompanyLink", type);
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCompanyLink(int id = 0)
        {
            CompanyLink type = db.CompanyLink.Find(id);
            
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteCompanyLink")]
        public ActionResult DeleteCompanyLinkConfirmed(int id)
        {
            CompanyLink type = db.CompanyLink.Find(id);
            db.CompanyLink.Remove(type);
            Company company = db.Companies.Find(type.CompanyId);
            company.Revision++;
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        #endregion
        #region People

        //
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreatePeople(int companyid = 0)
        {
            Company company = db.Companies.Find(companyid);
            if (company == null)
                HttpNotFound();
            People ppl = new People();
            ppl.CompanyId = companyid;
            List<int> types = new List<int>();
          
            ViewBag.PeopleTypeList = new MultiSelectList(db.PeopleTypes.ToList(), "PeopleTypeId", "Title", types);
            //ViewBag.PeopleTypeList = new SelectList(db.PeopleTypes.ToList(), "PeopleTypeId", "Title");
            //ViewBag.LanguageList = new SelectList(db.Language.ToList(), "LanguageId", "Title");
            return View(ppl);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreatePeople(People type)
        {
            if (type.PeopleTypes == null)
                type.PeopleTypes = new List<PeopleType>();
            foreach (string ptype in Request.Form["SelectPeopleType"].Split(','))
            {
                PeopleType peopleType = db.PeopleTypes.Find(Int16.Parse(ptype));
                type.PeopleTypes.Add(peopleType);
            }
         /*   if (type.PeopleTypeId == 0)
            {
                type.PeopleTypeId = Int16.Parse(this.Request.Form.Get("PeopleTypeList"));
            }
        */
            if (ModelState.IsValid)
            {
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.Peoples.Add(type);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = type.CompanyId });
            }
            List<int> types = new List<int>();
            foreach (var peopleType in type.PeopleTypes)
                types.Add(peopleType.PeopleTypeId);
            ViewBag.PeopleTypeList = new MultiSelectList(db.PeopleTypes.ToList(), "PeopleTypeId", "Title", types);
           // ViewBag.PeopleTypeList = new SelectList(db.PeopleTypes.ToList(), "PeopleTypeId", "Title", type.PeopleType.PeopleTypeId);
            return View(type);
        }

        //
        // GET: /Company/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult EditPeople(int id = 0)
        {
            //CompanyInformation type = db.CompanyInformation.Find(id);
            People type = db.Peoples.Include("PeopleTypes").Where(a => a.PeopleId == id).Single();
            //People type = db.Peoples.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            List<int> types = new List<int>();
            foreach (var peopleType in type.PeopleTypes)
                types.Add(peopleType.PeopleTypeId);
            ViewBag.PeopleTypeList = new MultiSelectList(db.PeopleTypes.ToList(), "PeopleTypeId", "Title", types);
            return View("CreatePeople", type);
        }

        //
        // POST: /Company/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditPeople(People type)
        {
                   
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("DELETE FROM PeopleJoinPeopleType WHERE PeopleId=" + type.PeopleId.ToString());
                db.Entry(type).State = EntityState.Modified;
                var ptypes = Request.Form["SelectPeopleType"].Split(',').ToList();
                type.PeopleTypes = new List<PeopleType>();
                foreach (string ptype in ptypes)
                {
                    PeopleType peopleType = db.PeopleTypes.Find(Int16.Parse(ptype));
                    type.PeopleTypes.Add(peopleType);
                }
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = type.CompanyId });
            }
            List<int> types = new List<int>();
            foreach (var peopleType in type.PeopleTypes)
                types.Add(peopleType.PeopleTypeId);
            ViewBag.PeopleTypeList = new MultiSelectList(db.PeopleTypes.ToList(), "PeopleTypeId", "Title", types);
            return View("CreateCompanyLink", type);
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeletePeople(int id = 0)
        {
            People type = db.Peoples.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeletePeople")]
        public ActionResult DeletePeopleConfirmed(int id)
        {
            People type = db.Peoples.Find(id);
            var peopleInfo = from a in db.PeopleInformation where a.PeopleId == id select a;
            foreach (var item in peopleInfo)
                db.PeopleInformation.Remove(item);
            db.Peoples.Remove(type);
            Company company = db.Companies.Find(type.CompanyId);
            company.Revision++;
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        #endregion
        #region PeopleInformation
        //
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreatePeopleInformation(int peopleid = 0, int languageid = 0)
        {
            People people = db.Peoples.Find(peopleid);
            Language lang = db.Language.Find(languageid);
            if (people == null || lang == null)
                HttpNotFound();
            PeopleInformation ppl = new PeopleInformation();
            var peopleInfo = from a in db.PeopleInformation
                             where a.PeopleId == peopleid
                             where a.LanguageId == languageid
                             select a;
            if (peopleInfo.Count() == 1)
                ppl = peopleInfo.Include(a=>a.Language).Include(a=>a.People).Single();
            else
            {
                ppl.PeopleId = peopleid;
                ppl.LanguageId = languageid;
                ppl.Language = lang;
                ppl.People = people;
            }
            return View(ppl);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreatePeopleInformation(PeopleInformation type)
        {
            
            if (ModelState.IsValid)
            {
                if (type.PeopleInformationId == 0)
                {
                    db.PeopleInformation.Add(type);
                }
                else
                {
                    db.Entry(type).State = EntityState.Modified;
                }
                Company company = db.Companies.Find(db.Peoples.Find(type.PeopleId).CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                People ppl = db.Peoples.Find(type.PeopleId);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = ppl.CompanyId });
            }
            
            return View(type);
        }
        #endregion
        #region NewsSource
        // 
        // GET: /Company/CreateNewsSource
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateNewsSource(int companyid = 0, int id = 0)
        {
            Company company = db.Companies.Find(companyid);
            if (company == null)
                HttpNotFound();
            NewsSource newsSource = new NewsSource();
            if (id == 0)
                newsSource.CompanyId = companyid;
            else
                newsSource = db.NewsSources.Find(id);
            ViewBag.NewsSourceTypeList = new SelectList(db.NewsSourceTypes.ToList(), "NewsSourceTypeId", "Title", newsSource.NewsSourceTypeId);
            ViewBag.LanguageList = new SelectList(db.Language.ToList(), "LanguageId", "Title", newsSource.LanguageId);
            return View(newsSource);
        }
        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult CreateNewsSource(NewsSource type)
        {
            type.LanguageId = Int16.Parse(this.Request.Form.Get("LanguageList"));
            type.NewsSourceTypeId = Int16.Parse(this.Request.Form.Get("NewsSourceTypeList"));
            if (ModelState.IsValid)
            {
                if (type.NewsSourceId == 0)
                {
                    db.NewsSources.Add(type);
                }
                else
                {
                    db.Entry(type).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Details", new { id = type.CompanyId });
            }

            ViewBag.LanguageList = new SelectList(db.Language.ToList(), "Culture", "Title");
            ViewBag.NewsSourceTypeList = new SelectList(db.NewsSourceTypes.ToList(), "NewsSourceTypeId", "Title", type.NewsSourceTypeId);
            return View(type);
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteNewsSource(int id = 0)
        {
            NewsSource type = db.NewsSources.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteNewsSource")]
        public ActionResult DeleteNewsSourceConfirmed(int id)
        {
            NewsSource type = db.NewsSources.Find(id);
            db.NewsSources.Remove(type);
                
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        #endregion
        #region Dividends
        [Authorize(Roles = "Administrator")]
        public ActionResult Dividends(int id)
        {
            var stocks = db.StockDividends.Where(a => a.StockSymbolId == id).ToList();
            ViewBag.StockSymbolId = id;
            return View(stocks);
        }

        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateDividend(int id=0, int stockSymbolId=0)
        {
            StockDividend type = new StockDividend();
            if (id != 0)
            {
                type = db.StockDividends.Find(id);
                if (type == null)
                    return HttpNotFound();
            }
            else
            {
                StockSymbol symbol = db.StockSymbols.Find(stockSymbolId);
                if (symbol == null)
                    return HttpNotFound();
                type.StockSymbolId = symbol.StockSymbolId;
            }
            return View(type);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("CreateDividend")]
        public ActionResult CreateDividend(StockDividend type)
        {
            if (ModelState.IsValid)
            {
                if (type.StockDividendId == 0)
                {
                    db.StockDividends.Add(type);
                }
                else
                {
                    db.Entry(type).State = EntityState.Modified;
                }
                Company company = db.Companies.Find(db.StockSymbols.Find(type.StockSymbolId).CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dividends", new { id = type.StockSymbolId });
            }

            return View(type);
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteDividend(int id = 0)
        {
            StockDividend type = db.StockDividends.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteDividend")]
        public ActionResult DeleteDividendConfirmed(int id)
        {
            StockDividend type = db.StockDividends.Find(id);
            db.StockDividends.Remove(type);

            db.SaveChanges();
            return RedirectToAction("Dividends", new { id = type.StockSymbolId });
        }
        #endregion
        #region Documents
        
        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateDocument(int id = 0, int companyid = 0, int documenttypeid = 0, int languageid = 0)
        {
            OPIMsys.Models.Document type = new OPIMsys.Models.Document();
            
            if (id != 0)
            {
                type = db.Documents.Find(id);
                if (type == null)
                    return HttpNotFound();
            }
            else
            {
                Company company = db.Companies.Find(companyid);
                if (company == null) return HttpNotFound();
                Language lang = db.Language.Find(languageid);
                if (lang == null) return HttpNotFound();
                DocumentType doctype = db.DocumentTypes.Find(documenttypeid);
                if (doctype == null) return HttpNotFound();

                type.LanguageId = lang.LanguageId;
                type.DocumentTypeId = doctype.DocumentTypeId;
                type.CompanyId = company.CompanyId;
                type.PubDate = DateTime.Today;
                type.ThumbnailLink = "temp";
                
            }
            
            return View(type);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("CreateDocument")]
        public ActionResult CreateDocument(OPIMsys.Models.Document type)
        {
           
            
            if (ModelState.IsValid)
            {
                if (type.DocumentId == 0)
                {
                    db.Documents.Add(type);
                    
                }
                else
                {
                    db.Entry(type).State = EntityState.Modified;
                }
                
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                if (type.Link.Substring(type.Link.Length - 3) == "pdf")
                {
                    if (type.Link.StartsWith("http://"))
                        type.Link = type.Link.Substring(7);
                    else if (type.Link.StartsWith("https://"))
                        type.Link = type.Link.Substring(8);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://bmirphpdev2.com/pdfapi/test.php?url=http://" + type.Link);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream receiveStream = response.GetResponseStream();
                    // read the stream
                   

                    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                    var blobStorage = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobStorage.GetContainerReference("images");
                    if (container.CreateIfNotExist())
                    {
                        // configure container for public access
                        var permissions = container.GetPermissions();
                        permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                        container.SetPermissions(permissions);
                    }

                    //Azure
                    string uniqueBlobName = string.Format("images/docthumb-{0}.png", type.DocumentId.ToString());
                    CloudBlockBlob blob = blobStorage.GetBlockBlobReference(uniqueBlobName);
                    blob.Properties.ContentType = "image/png";
                    blob.UploadFromStream(receiveStream);
                    type.ThumbnailLink = blob.Uri.ToString();

                    receiveStream.Close();
                    response.Close();
          
                    db.Entry(type).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Details", new { id = type.CompanyId });
            }

            return View(type);
        }
        //
        // GET: /Company/DeleteDocument/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteDocument(int id = 0)
        {
            Document type = db.Documents.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/DeleteDocument/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteDocument")]
        public ActionResult DeleteDocumentConfirmed(int id)
        {
            Document type = db.Documents.Find(id);
            db.Documents.Remove(type);

            string path = Server.MapPath("~/Content/Documents/");
            System.IO.File.Delete(path + type.DocumentId + ".jpg");
            db.SaveChanges();
            return RedirectToAction("Details", new { id = type.CompanyId });
        }
        #endregion
        #region Events
        [Authorize(Roles = "Administrator")]
        public ActionResult Events(int id)
        {
            var events = db.Events.Where(a => a.CompanyId == id).ToList();
            ViewBag.CompanyId = id;
            ViewBag.Languages = db.Language.ToList();
            return View(events);
        }

        // GET: /Company/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateEvent(int id = 0, int companyid = 0)
        {
            Event type = new Event();

            if (id != 0)
            {
                type = db.Events.Find(id);
                if (type == null)
                    return HttpNotFound();
            }
            else
            {
                Company company = db.Companies.Find(companyid);
                if (company == null)
                    return HttpNotFound();
                type.CompanyId = company.CompanyId;
                try
                {
                    type.EventSourceId = db.EventSources
                                                .Where(a => a.EventSourceType.Title == "Manual")
                                                .Where(a => a.Language.Culture == "en")
                                                .Where(a => a.CompanyId == type.CompanyId)
                                                .Single().EventSourceId;
                }
                catch (Exception err)
                {
                    db.EventSources.Add(new EventSource
                    {
                        EventSourceTypeId = db.EventSourceTypes.Where(a => a.Title == "Manual").Single().EventSourceTypeId,
                        LanguageId = db.Language.Where(a => a.Culture == "en").Single().LanguageId,
                        Link = "Manual",
                        CompanyId = type.CompanyId
                    });
                    db.SaveChanges();
                    type.EventSourceId = db.EventSources.Where(a => a.EventSourceType.Title == "Manual").Where(a => a.Language.Culture == "en").Single().EventSourceId;
                }
            }
            ViewBag.EventCategoriesList = new SelectList(db.EventCategories.ToList(), "EventCategoryId", "Title", type.EventCategoryId);
            return View(type);
        }

        //
        // POST: /Company/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("CreateEvent")]
        public ActionResult CreateEvent(Event type)
        {
            if (type.EventCategoryId == 0)
                type.EventCategoryId = Int16.Parse(this.Request.Form.Get("EventCategoriesList"));
            EventDetail details = new EventDetail();
            if (this.Request.Form.Get("EventDetailId") != null)
            {
                details = db.EventDetail.Find(Int16.Parse(this.Request.Form.Get("EventDetailId")));
                details.Summary = this.Request.Form.Get("Summary");
                details.Description = this.Request.Form.Get("Description");
            }
            else
            {
                details = new EventDetail
                {
                    Summary = this.Request.Form.Get("Summary"),
                    Description = this.Request.Form.Get("Description"),
                    LanguageId = db.Language.Where(a => a.Culture == "en").Single().LanguageId
                };

            }

            if (ModelState.IsValid)
            {
                if (type.EventId == 0)
                {
                    db.Events.Add(type);
                    details.EventId = type.EventId;
                    db.EventDetail.Add(details);
                }
                else
                {
                    db.Entry(type).State = EntityState.Modified;
                    db.Entry(details).State = EntityState.Modified;
                }
                Company company = db.Companies.Find(type.CompanyId);
                company.Revision++;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Events", new { id = type.CompanyId });
            }

            return View(type);
        }
        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteEvent(int id = 0)
        {
            Event type = db.Events.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteEvent")]
        public ActionResult DeleteEventConfirmed(int id)
        {
            Event type = db.Events.Find(id);
            db.Events.Remove(type);

            db.SaveChanges();
            return RedirectToAction("Events", new { id = type.CompanyId });
        }
        #endregion
        #region Markets
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateMarketComparison(int id)
        {
            ViewBag.Symbols = db.StockSymbols.ToList();
            ViewBag.SelectedSymbols = db.CompanyMarketComparisons.Where(a => a.CompanyId == id).ToList();
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("UpdateMarketComparison")]
        public ActionResult UpdateMarketComparisonConfirmed(int id)
        {
            Company type = db.Companies.Find(id);
            var symbols = db.StockSymbols;
            foreach (var delcomp in db.CompanyMarketComparisons.Where(a => a.CompanyId == type.CompanyId))
            {
                db.CompanyMarketComparisons.Remove(delcomp);
            }
            db.SaveChanges();
            foreach (var symbol in symbols)
            {
                if (Request.Form["StockSymbolId" + symbol.StockSymbolId.ToString()] == "true,false" || Request.Form["StockSymbolId" + symbol.StockSymbolId.ToString()] == "true")
                {
                    CompanyMarketComparison dComp = new CompanyMarketComparison();
                    dComp.CompanyId = type.CompanyId;
                    dComp.StockSymbolId = symbol.StockSymbolId;
                    db.CompanyMarketComparisons.Add(dComp);
                }

            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        #endregion
        #region Peers
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdatePeers(int id)
        {
            ViewBag.Companies = db.Companies.ToList();
            ViewBag.SelectedCompanies = db.CompanyPeers.Where(a => a.CompanyId == id).ToList();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("UpdatePeers")]
        public ActionResult UpdatePeersConfirmed(int id)
        {
            Company type = db.Companies.Find(id);
            var companies = db.Companies;
            foreach (var delcomp in db.CompanyPeers.Where(a => a.CompanyId == type.CompanyId))
            {
                db.CompanyPeers.Remove(delcomp);
            }
            db.SaveChanges();
            foreach (Company company in companies)
            {
                if (Request.Form["CompanyId" + company.CompanyId.ToString()] == "true,false" || Request.Form["CompanyId" + company.CompanyId.ToString()] == "true")
                {
                    CompanyPeer dComp = new CompanyPeer();
                    dComp.CompanyId = type.CompanyId;
                    dComp.PeerId = company.CompanyId;
                    db.CompanyPeers.Add(dComp);
                }

            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }
        #endregion
        #region Groups
        [Authorize(Roles = "Administrator")]
        public ActionResult Groups()
        {
            
            return View(db.Groups.ToList());
        }

        public ActionResult UpdateGroup(int id)
        {
            
            Group type = new Group();
            if (id != 0)
            {
                type = db.Groups.Find(id);
                if (type == null)
                    return HttpNotFound();
                ViewBag.SelectedCompanies = db.CompanyGroups.Where(a => a.GroupId == type.GroupId).ToList();
            }
            ViewBag.Companies = db.Companies.OrderBy(a => a.Name).ToList();
            return View(type);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("UpdateGroup")]
        public ActionResult UpdateGroupConfirmed(Group type)
        {
            if (ModelState.IsValid)
            {
                if (type.GroupId == 0)
                {
                    db.Groups.Add(type);
                }
                else
                {
                    db.Entry(type).State = EntityState.Modified;
                }
                db.SaveChanges();
            
            
                var companies = db.Companies;
                foreach (var delcomp in db.CompanyGroups.Where(a => a.GroupId == type.GroupId))
                {
                    db.CompanyGroups.Remove(delcomp);
                }
                db.SaveChanges();
                foreach (Company company in companies)
                {
                    if (Request.Form["CompanyId" + company.CompanyId.ToString()] == "true,false" || Request.Form["CompanyId" + company.CompanyId.ToString()] == "true")
                    {
                        CompanyGroup dComp = new CompanyGroup();
                        dComp.CompanyId = company.CompanyId;
                        dComp.GroupId = type.GroupId;
                        db.CompanyGroups.Add(dComp);
                    }

                }
                db.SaveChanges();
                return RedirectToAction("Groups");
            }
            ViewBag.SelectedCompanies = db.CompanyGroups.Where(a => a.GroupId == type.GroupId).ToList();
            ViewBag.Companies = db.Companies.OrderBy(a => a.Name).ToList();
            return View(type);

        }

        //
        // GET: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteGroup(int id = 0)
        {
            Group type = db.Groups.Find(id);

            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Company/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult DeleteGroupConfirmed(int id)
        {
            Group type = db.Groups.Find(id);
            db.Groups.Remove(type);

            db.SaveChanges();
            return RedirectToAction("Groups");
        }
        #endregion
        /*    void TextSettings_ResolveFont(TallComponents.PDF.Rasterizer.Configuration.TextRenderSettings sender, TallComponents.PDF.Rasterizer.Fonts.ResolveFontEventArgs args)
        {
            System.Diagnostics.Trace.WriteLine(String.Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath));

            if (args.FontLocation != TallComponents.PDF.Rasterizer.Fonts.FontLocation.System)
            {
                switch (args.PdfFontName)
                {
                    case "Times-Roman":
                        args.SystemFontName = "Times New Roman";
                        break;
                    case "Times-Bold":
                        args.SystemFontName = "Times New Roman";
                        args.Bold = true;
                        break;
                    case "Times-Italic":
                        args.SystemFontName = "Times New Roman";
                        args.Italic = true;
                        break;
                    case "Helvetica":
                        args.SystemFontName = "Arial Unicode MS";
                        break;
                    case "Helvetica-Bold":
                        args.SystemFontName = "Arial Unicode MS";
                        args.Bold = true;
                        break;
                    case "Helvetica-Italic":
                        args.SystemFontName = "Arial Unicode MS";
                        args.Italic = true;
                        break;
                }
            }
        }
        */
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}