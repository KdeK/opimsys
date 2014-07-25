using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPIMsys.Models;

namespace OPIMsys.Controllers
{
    public class SettingsController : Controller
    {
        private OPIMsysContext db = new OPIMsysContext();

        //
        // GET: /Settings/

        public ActionResult Index()
        {
            return View();
        }
        #region PeopleType ******************************************************************************************************
        //
        // GET: /Settings/People/

        public ActionResult PeopleType()
        {
            return View(db.PeopleTypes.ToList());
        }

        //
        // GET: /Settings/CreatePeopleType
        public ActionResult CreatePeopleType()
        {
            return View();
        }

        //
        // POST: /Settings/CreatePeopleType
        [HttpPost]
        public ActionResult CreatePeopleType(PeopleType type)
        {
            if (ModelState.IsValid)
            {
                db.PeopleTypes.Add(type);
                db.SaveChanges();
                return RedirectToAction("PeopleType");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditPeopleType/5

        public ActionResult EditPeopleType(int id = 0)
        {
            PeopleType type = db.PeopleTypes.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditPeopleType/5

        [HttpPost]
        public ActionResult EditPeopleType(PeopleType type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PeopleType");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeletePeopleType/5

        public ActionResult DeletePeopleType(int id = 0)
        {
            PeopleType type = db.PeopleTypes.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeletePeopleType/5

        [HttpPost, ActionName("DeletePeopleType")]
        public ActionResult DeletePeopleTypeConfirmed(int id)
        {
            PeopleType type = db.PeopleTypes.Find(id);
            db.PeopleTypes.Remove(type);
            db.SaveChanges();
            return RedirectToAction("PeopleType");
        }
        #endregion
        #region NewsType ********************************************************************************************************
        //
        // GET: /Settings/NewsType/

        public ActionResult NewsType()
        {
            return View(db.NewsType.ToList());
        }

        //
        // GET: /Settings/CreateNewsType
        public ActionResult CreateNewsType()
        {
            return View();
        }

        //
        // POST: /Settings/CreateNewsType
        [HttpPost]
        public ActionResult CreateNewsType(NewsType type)
        {
            if (ModelState.IsValid)
            {
                db.NewsType.Add(type);
                db.SaveChanges();
                return RedirectToAction("NewsType");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditNewsType/5

        public ActionResult EditNewsType(int id = 0)
        {
            NewsType type = db.NewsType.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditNewsType/5

        [HttpPost]
        public ActionResult EditNewsType(NewsType type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewsType");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteNewsType/5

        public ActionResult DeleteNewsType(int id = 0)
        {
            NewsType type = db.NewsType.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteNewsType/5

        [HttpPost, ActionName("DeleteNewsType")]
        public ActionResult DeleteNewsTypeConfirmed(int id)
        {
            NewsType type = db.NewsType.Find(id);
            db.NewsType.Remove(type);
            db.SaveChanges();
            return RedirectToAction("NewsType");
        }
        #endregion
        #region DocumentType ****************************************************************************************************
        //
        // GET: /Settings/DocumentType/

        public ActionResult DocumentType()
        {
            return View(db.DocumentTypes.ToList());
        }

        //
        // GET: /Settings/CreateDocumentType
        public ActionResult CreateDocumentType()
        {
            return View();
        }

        //
        // POST: /Settings/CreateDocumentType
        [HttpPost]
        public ActionResult CreateDocumentType(DocumentType type)
        {
            if (ModelState.IsValid)
            {
                db.DocumentTypes.Add(type);
                db.SaveChanges();
                return RedirectToAction("DocumentType");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditDocumentType/5

        public ActionResult EditDocumentType(int id = 0)
        {
            DocumentType type = db.DocumentTypes.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditDocumentType/5

        [HttpPost]
        public ActionResult EditDocumentType(DocumentType type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DocumentType");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteDocumentType/5

        public ActionResult DeleteDocumentType(int id = 0)
        {
            DocumentType type = db.DocumentTypes.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteDocumentType/5

        [HttpPost, ActionName("DeleteDocumentType")]
        public ActionResult DeleteDocumentTypeConfirmed(int id)
        {
            DocumentType type = db.DocumentTypes.Find(id);
            db.DocumentTypes.Remove(type);
            db.SaveChanges();
            return RedirectToAction("DocumentType");
        }
        #endregion
        #region CompanyLinkType *************************************************************************************************
        //
        // GET: /Settings/CompanyLinkType/

        public ActionResult CompanyLinkType()
        {
            return View(db.CompanyLinkType.ToList());
        }

        //
        // GET: /Settings/CreateCompanyLinkType
        public ActionResult CreateCompanyLinkType()
        {
            return View();
        }

        //
        // POST: /Settings/CreateCompanyLinkType
        [HttpPost]
        public ActionResult CreateCompanyLinkType(CompanyLinkType type)
        {
            if (ModelState.IsValid)
            {
                db.CompanyLinkType.Add(type);
                db.SaveChanges();
                return RedirectToAction("CompanyLinkType");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditCompanyLinkType/5

        public ActionResult EditCompanyLinkType(int id = 0)
        {
            CompanyLinkType type = db.CompanyLinkType.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditCompanyLinkType/5

        [HttpPost]
        public ActionResult EditCompanyLinkType(CompanyLinkType type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CompanyLinkType");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteCompanyLinkType/5

        public ActionResult DeleteCompanyLinkType(int id = 0)
        {
            CompanyLinkType type = db.CompanyLinkType.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteCompanyLinkType/5

        [HttpPost, ActionName("DeleteCompanyLinkType")]
        public ActionResult DeleteCompanyLinkTypeConfirmed(int id)
        {
            CompanyLinkType type = db.CompanyLinkType.Find(id);
            db.CompanyLinkType.Remove(type);
            db.SaveChanges();
            return RedirectToAction("CompanyLinkType");
        }
        #endregion
        #region Event Category **************************************************************************************************
        //
        // GET: /Settings/EventCategory/

        public ActionResult EventCategory()
        {
            return View(db.EventCategories.ToList());
        }

        //
        // GET: /Settings/CreateEventCategory
        public ActionResult CreateEventCategory()
        {
            return View();
        }

        //
        // POST: /Settings/CreateEventCategory
        [HttpPost]
        public ActionResult CreateEventCategory(EventCategory type)
        {
            if (ModelState.IsValid)
            {
                db.EventCategories.Add(type);
                db.SaveChanges();
                return RedirectToAction("EventCategory");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditEventCategory/5

        public ActionResult EditEventCategory(int id = 0)
        {
            EventCategory type = db.EventCategories.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditEventCategory/5

        [HttpPost]
        public ActionResult EditEventCategory(EventCategory type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EventCategory");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteEventCategory/5

        public ActionResult DeleteEventCategory(int id = 0)
        {
            EventCategory type = db.EventCategories.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteEventCategory/5

        [HttpPost, ActionName("DeleteEventCategory")]
        public ActionResult DeleteEventCategoryConfirmed(int id)
        {
            EventCategory type = db.EventCategories.Find(id);
            db.EventCategories.Remove(type);
            db.SaveChanges();
            return RedirectToAction("EventCategory");
        }
        #endregion
        #region Event Source Type ***********************************************************************************************
        //
        // GET: /Settings/EventSourceType/

        public ActionResult EventSourceType()
        {
            return View(db.EventSourceTypes.ToList());
        }

        //
        // GET: /Settings/CreateEventSourceType
        public ActionResult CreateEventSourceType()
        {
            return View();
        }

        //
        // POST: /Settings/CreateEventSourceType
        [HttpPost]
        public ActionResult CreateEventSourceType(EventSourceType type)
        {
            if (ModelState.IsValid)
            {
                db.EventSourceTypes.Add(type);
                db.SaveChanges();
                return RedirectToAction("EventSourceType");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditEventSourceType/5

        public ActionResult EditEventSourceType(int id = 0)
        {
            EventSourceType type = db.EventSourceTypes.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditEventSourceType/5

        [HttpPost]
        public ActionResult EditEventSourceType(EventSourceType type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EventSourceType");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteEventSourceType/5

        public ActionResult DeleteEventSourceType(int id = 0)
        {
            EventSourceType type = db.EventSourceTypes.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteEventSourceType/5

        [HttpPost, ActionName("DeleteEventSourceType")]
        public ActionResult DeleteEventSourceTypeConfirmed(int id)
        {
            EventSourceType type = db.EventSourceTypes.Find(id);
            db.EventSourceTypes.Remove(type);
            db.SaveChanges();
            return RedirectToAction("EventSourceType");
        }
        #endregion
        #region Languages *******************************************************************************************************
        //
        // GET: /Settings/Languages/

        public ActionResult Languages()
        {
            return View(db.Language.ToList());
        }

        //
        // GET: /Settings/CreateLanguage
        public ActionResult CreateLanguage()
        {
            return View();
        }

        //
        // POST: /Settings/CreateLanguage
        [HttpPost]
        public ActionResult CreateLanguage(Language type)
        {
            if (ModelState.IsValid)
            {
                db.Language.Add(type);
                db.SaveChanges();
                return RedirectToAction("Languages");
            }

            return View(type);
        }
        //
        // GET: /Settings/EditLanguage/5

        public ActionResult EditLanguage(int id = 0)
        {
            Language type = db.Language.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditLanguage/5

        [HttpPost]
        public ActionResult EditLanguage(Language type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Languages");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteLanguage/5

        public ActionResult DeleteLanguage(int id=0)
        {
            Language type = db.Language.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteLanguage/5

        [HttpPost, ActionName("DeleteLanguage")]
        public ActionResult DeleteLanguageConfirmed(int id)
        {
            Language type = db.Language.Find(id);
            db.Language.Remove(type);
            db.SaveChanges();
            return RedirectToAction("Languages");
        }
        #endregion
        #region Markets *********************************************************************************************************
        //
        // GET: /Settings/Markets/

        public ActionResult Markets()
        {
            return View(db.Markets.ToList());
        }
        //
        // GET: /Settings/CreateMarket

        public ActionResult CreateMarket()
        {
            return View();
        }

        //
        // POST: /Settings/CreateMarket

        [HttpPost]
        public ActionResult CreateMarket(Market type)
        {
            if (ModelState.IsValid)
            {
                db.Markets.Add(type);
                db.SaveChanges();
                return RedirectToAction("Markets");
            }

            return View(type);
        }

        //
        // GET: /Settings/EditMarket/5

        public ActionResult EditMarket(int id = 0)
        {
            Market type = db.Markets.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/EditMarket/5

        [HttpPost]
        public ActionResult EditMarket(Market type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Markets");
            }
            return View(type);
        }

        //
        // GET: /Settings/DeleteMarket/5

        public ActionResult DeleteMarket(int id = 0)
        {
            Market type = db.Markets.Find(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        //
        // POST: /Settings/DeleteMarket/5

        [HttpPost, ActionName("DeleteMarket")]
        public ActionResult DeleteMarketConfirmed(int id)
        {
            Market type = db.Markets.Find(id);
            db.Markets.Remove(type);
            db.SaveChanges();
            return RedirectToAction("Markets");
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        
    }
}