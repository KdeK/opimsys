using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPIMsys.Models;
using System.Net.Http;
using System.Net;

namespace OPIMsys.Controllers
{
    public class TableController : Controller
    {
        private OPIMsysContext db = new OPIMsysContext();

        //
        // GET: /Table/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.CompanyDataTypes.ToList());
        }
        [Authorize(Roles = "Administrator")]
        public HttpResponseMessage DeleteRowData(int id = 0, string publishDate = "")
        {
            DateTime pubDate = DateTime.Parse(publishDate);
            var data = db.CompanyData.Where(a => a.CompanyDataVariableId == id).Where(a => a.publishDate == pubDate);
            foreach (CompanyData value in data)
            {
                db.CompanyData.Remove(value);
            }
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [Authorize(Roles = "Administrator")]
        public HttpResponseMessage UpdateData(int id=0, int companyId=0, decimal value=0, string publishDate="")
        {
            DateTime pubDate = DateTime.Parse(publishDate);
            CompanyData data = db.CompanyData.Where(a => a.CompanyDataVariableId==id).Where(a=> a.CompanyId==companyId).Where(a=> a.publishDate==pubDate).FirstOrDefault();
            if (data == null)
            {
                Company comp = db.Companies.Find(companyId);
                CompanyDataVariable compVar = db.CompanyDataVariables.Find(id);
                if(comp == null || compVar == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                data = new CompanyData { CompanyId = comp.CompanyId, publishDate = pubDate, CompanyDataVariableId = compVar.CompanyDataVariableId, CompanyDataId = 0 };
            }
            data.Value = value;
            if (data.CompanyDataId != 0)
                db.Entry(data).State = EntityState.Modified;
            else
                db.CompanyData.Add(data);
            
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
        //
        // GET: /Table/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Data(int id = 0)
        {
            CompanyDataType companydatatype = db.CompanyDataTypes.Find(id);
            if (companydatatype == null)
            {
                return HttpNotFound();
            }
            var companies =  db.CompanyDataTypeCompanies.Where(a => a.CompanyDataTypeId == id).ToList();
            var variables = db.CompanyDataVariables.Where(a => a.CompanyDataTypeId == id).OrderBy(a => a.Title).ToList();
            IEnumerable<int> varIds = variables.Select(a => a.CompanyDataVariableId);
            var values = db.CompanyData.Where(a => varIds.Contains(a.CompanyDataVariableId));
            var dates = values.Select(a => a.publishDate).Distinct().ToList();
            //dates.Add(DateTime.Now);
            string jsArray = "var valueArr = [\n";
            foreach (var company in companies)
            {
                jsArray += "{companyId:" + company.CompanyId + ",name: \""+company.Company.Name+"\", dataset: [";
                foreach (DateTime pubDate in dates)
                {
                    jsArray += "{ date: \""+pubDate.ToShortDateString()+"\", values: [";
                    foreach (var variable in variables)
                    {
                        decimal val = 0;
                        var retVal = values.Where(a => a.CompanyId == company.CompanyId).Where(a => a.CompanyDataVariableId == variable.CompanyDataVariableId).Where(a => a.publishDate == pubDate);
                        if(retVal.Count() != 0)
                            val = retVal.First().Value;
                        jsArray += "{title:\""+variable.Title+"\", value:" + val.ToString() +", variableId:"+variable.CompanyDataVariableId+"},";
                    }
                    jsArray = jsArray.Substring(0, jsArray.Length - 1);
                    jsArray += "]},";
                }
                jsArray = jsArray.Substring(0, jsArray.Length - 1);
                jsArray += "]},";
            }
            jsArray = jsArray.Substring(0, jsArray.Length - 1);
            jsArray += "];";
                
            ViewBag.jsArray = jsArray;
            return View(companydatatype);
        }

     

        //
        // GET: /Table/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {
            CompanyDataType companydatatype;
            if (id != 0)
            {
                companydatatype = db.CompanyDataTypes.Find(id);
                if (companydatatype == null)
                {
                    return HttpNotFound();
                }
            }
            else
                companydatatype = new CompanyDataType();

            string variables = "";
            foreach (var variable in db.CompanyDataVariables.Where(a => a.CompanyDataTypeId == companydatatype.CompanyDataTypeId))
                variables += variable.Title + ";";
            if (variables != "")
                variables = variables.Substring(0, variables.Length - 1);
            ViewBag.ListVariables = variables;
            ViewBag.Companies = db.Companies.OrderBy(a => a.Name).ToList();
            ViewBag.SelectedCompanies = db.CompanyDataTypeCompanies.Where(a => a.CompanyDataTypeId == id).ToList();
            return View(companydatatype);
        }

        //
        // POST: /Table/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyDataType companydatatype)
        {
            if (ModelState.IsValid)
            {
                var companies = db.Companies;
                foreach(var delcomp in db.CompanyDataTypeCompanies.Where(a => a.CompanyDataTypeId == companydatatype.CompanyDataTypeId))
                {
                    db.CompanyDataTypeCompanies.Remove(delcomp);
                }
                db.SaveChanges();
                if (companydatatype.CompanyDataTypeId == 0)
                    db.CompanyDataTypes.Add(companydatatype);
                else
                    db.Entry(companydatatype).State = EntityState.Modified;
                string[] variables = Request.Form["ListVariables"].Split(';');
                foreach (string variable in variables)
                {
                    if (db.CompanyDataVariables.Where(a => a.CompanyDataTypeId == companydatatype.CompanyDataTypeId).Where(a => a.Title == variable).Count() == 0)
                    {
                        CompanyDataVariable dvar = new CompanyDataVariable();
                        dvar.Title = variable;
                        dvar.CompanyDataTypeId = companydatatype.CompanyDataTypeId;
                        db.CompanyDataVariables.Add(dvar);
                    }
                }
                foreach (Company company in companies)
                {
                    if (Request.Form["CompanyId" + company.CompanyId.ToString()] == "true,false" || Request.Form["CompanyId" + company.CompanyId.ToString()] == "true")
                    {
                        CompanyDataTypeCompany dComp = new CompanyDataTypeCompany();
                        dComp.CompanyId = company.CompanyId;
                        dComp.CompanyDataTypeId = companydatatype.CompanyDataTypeId;
                        db.CompanyDataTypeCompanies.Add(dComp);
                        
                    }

                }
                db.SaveChanges();
                
                
                return RedirectToAction("Index");
            }
            string vars = "";
            foreach (var variable in db.CompanyDataVariables.Where(a => a.CompanyDataTypeId == companydatatype.CompanyDataTypeId))
                vars += variable.Title + ";";
            if (vars != "")
                vars = vars.Substring(0, vars.Length - 1);
            ViewBag.ListVariables = vars;

            ViewBag.Companies = db.Companies.ToList();
            ViewBag.SelectedCompanies = db.CompanyDataTypeCompanies.Where(a => a.CompanyDataTypeId == companydatatype.CompanyDataTypeId).ToList();

            return View(companydatatype);
        }

        //
        // GET: /Table/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {
            CompanyDataType companydatatype = db.CompanyDataTypes.Find(id);
            if (companydatatype == null)
            {
                return HttpNotFound();
            }
            return View(companydatatype);
        }

        //
        // POST: /Table/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyDataType companydatatype = db.CompanyDataTypes.Find(id);
            db.CompanyDataTypes.Remove(companydatatype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}