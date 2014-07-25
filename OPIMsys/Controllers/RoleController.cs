using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPIMsys.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace OPIMsys.Controllers
{
    public class RoleController : Controller
    {
        private OPIMsysContext db = new OPIMsysContext();

        //
        // GET: /Role/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            List<RoleModel> roles = new List<RoleModel>();
            foreach (string roleName in Roles.GetAllRoles())
            {
                RoleModel newRole = new RoleModel();
                newRole.RoleName = roleName;
                roles.Add(newRole);
            }
            return View(roles);
        }

        //
        // GET: /Role/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Role/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(RoleModel rolemodel)
        {
            if (ModelState.IsValid)
            {
                if (!Roles.RoleExists(rolemodel.RoleName))
                    Roles.CreateRole(rolemodel.RoleName);
                return RedirectToAction("Index");
            }
            return View(rolemodel);
        }

        //
        // GET: /Role/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string name)
        {
            if (!Roles.RoleExists(name))
            {
                throw new HttpException(404, "Not Found");
            }
            RoleModel rolemodel = new RoleModel();
            rolemodel.RoleName = name;
            ViewBag.oldRole = name;
            return View(rolemodel);
        }

        //
        // POST: /Role/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(RoleModel rolemodel)
        {
            if (ModelState.IsValid)
            {
                string oldRoleName = Request.Form["oldRole"].ToString();
                if (oldRoleName != rolemodel.RoleName)
                {
                    Roles.CreateRole(rolemodel.RoleName);
                    string[] usernames = Roles.GetUsersInRole(oldRoleName);
                    if (usernames.Count() > 0)
                    {
                        Roles.AddUsersToRole(usernames, rolemodel.RoleName);
                        Roles.RemoveUsersFromRole(usernames, oldRoleName);
                    }
                    Roles.DeleteRole(oldRoleName);
                }
                return RedirectToAction("Index");
            }
            ViewBag.oldRole = Request.Form["oldRole"].ToString();
            return View(rolemodel);
        }

        //
        // GET: /Role/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string name)
        {
            if (!Roles.RoleExists(name))
            {
                throw new HttpException(404, "Not Found");
            }
            RoleModel rolemodel = new RoleModel();
            rolemodel.RoleName = name;
            return View(rolemodel);
        }

        //
        // POST: /Role/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string name)
        {
            string[] usernames = Roles.GetUsersInRole(name);
            if (usernames.Count() > 0)
                Roles.RemoveUsersFromRole(usernames, name);
            Roles.DeleteRole(name);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}