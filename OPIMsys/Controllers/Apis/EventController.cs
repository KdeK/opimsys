using OPIMsys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OPIMsys.Controllers.Apis
{
    public class EventController : ApiController
    {
        private OPIMsysContext db = new OPIMsysContext();

        // PUT api/News/5
        public HttpResponseMessage PutEvent(string id, EventApi newapi)
        {
            AccountApiKey apiUser = GetAccount();
            EventSourceType eventsType = db.EventSourceTypes.Where(a => a.Title == "API").Single();
            EventSource eventsSource = db.EventSources.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.EventSourceTypeId == eventsType.EventSourceTypeId).Single();
            Event newEvent = new Event();
            EventCategory eventCategory = new EventCategory();
            Language lang = new Language();
            try
            {
                newEvent = db.Events.Include("EventDetails").Where(a => a.CompanyId == apiUser.CompanyId).Where(c => c.EventSourceId == eventsSource.EventSourceId).Where(b => b.SourceId == id).Single();
                eventCategory = db.EventCategories.Where(a => a.Title == newapi.Category).Single();
                lang = db.Language.Where(a => a.Culture == newapi.Language).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            newEvent.StartTime = newapi.StartTime;
            newEvent.EndTime = newapi.StartTime.AddMinutes((double)newapi.DurationM);
            EventDetail eventDetails = new EventDetail();
            var eventDetailCnt = (from a in newEvent.EventDetails
                                        where a.LanguageId == lang.LanguageId
                                        select a);
            if (eventDetailCnt.Count() == 1)
                eventDetails = eventDetailCnt.First();
            eventDetails.Description = newapi.Description;
            eventDetails.LanguageId = lang.LanguageId;
            eventDetails.Summary = newapi.Summary;
            eventDetails.EventId = newEvent.EventId;
            

            if (ModelState.IsValid)
            {
                //db.Entry(news).State = EntityState.Modified;

                try
                {
                    if (eventDetailCnt.Count() == 0)
                        db.EventDetail.Add(eventDetails);
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
        public HttpResponseMessage PostEvent(string id, EventApi newapi)
        {
            AccountApiKey apiUser = GetAccount();
            EventSourceType eventsType = db.EventSourceTypes.Where(a => a.Title == "API").Single();
            EventSource eventsSource = db.EventSources.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.EventSourceTypeId == eventsType.EventSourceTypeId).Single();
            Event newEvent = new Event();
            EventCategory eventCategory = new EventCategory();
            Language lang = new Language();
            try
            {
                eventCategory = db.EventCategories.Where(a => a.Title == newapi.Category).Single();
                lang = db.Language.Where(a => a.Culture == newapi.Language).Single();
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            newEvent.StartTime = newapi.StartTime;
            newEvent.EndTime = newapi.StartTime.AddMinutes((double)newapi.DurationM);
            newEvent.CompanyId = apiUser.CompanyId;
            newEvent.EventCategoryId = eventCategory.EventCategoryId;
            newEvent.SourceId = id;
            
            EventDetail eventDetails = new EventDetail();
            eventDetails.Description = newapi.Description;
            eventDetails.LanguageId = lang.LanguageId;
            eventDetails.Summary = newapi.Summary;
            eventDetails.EventId = newEvent.EventId;
            
            if (ModelState.IsValid)
            {
                db.Events.Add(newEvent);
                db.EventDetail.Add(eventDetails);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/News/5
        public HttpResponseMessage DeleteEvent(string id)
        {
            AccountApiKey apiUser = GetAccount();
            EventSourceType eventsType = db.EventSourceTypes.Where(a => a.Title == "API").Single();
            EventSource eventsSource = db.EventSources.Where(a => a.CompanyId == apiUser.CompanyId).Where(a => a.EventSourceTypeId == eventsType.EventSourceTypeId).Single();
            Event newEvent = new Event();
            try
            {
                newEvent = db.Events.Where(a => a.CompanyId == apiUser.CompanyId).Where(c => c.EventSourceId == eventsSource.EventSourceId).Where(b => b.SourceId == id).Single();
                
            }
            catch (Exception err)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var eventDetails = db.EventDetail.Where(a => a.EventId == newEvent.EventId);
            foreach (EventDetail eventDetail in eventDetails)
                db.EventDetail.Remove(eventDetail);

            db.Events.Remove(newEvent);
            
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
