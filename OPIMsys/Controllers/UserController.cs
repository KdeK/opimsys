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
    public class UserController : Controller
    {
        private OPIMsysContext db = new OPIMsysContext();

        //
        // GET: /Role/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList());
        }


        //
        // GET: /Role/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.Roles = Roles.GetAllRoles();
         //   ViewBag.Departments = db.Departments.ToList().OrderBy(p => p.DepartmentName);
            return View();
        }

        //
        // POST: /Role/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { FullName = model.FullName, Email = model.Email});
                    //WebSecurity.Login(model.UserName, model.Password);
                    UserProfile user = db.UserProfiles.Where(a => a.UserName == model.UserName).First();
             //       user = UpdateDepartments(user);
                    user = UpdateRoles(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }

            }
            return View(model);
        }

        //
        // GET: /Role/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {
            UserProfile profile = db.UserProfiles.Find(id);
            if (profile == null)
            {
                throw new HttpException(404, "Not Found");
            }

            ViewBag.Roles = Roles.GetAllRoles();
            ViewBag.SelectedRoles = Roles.GetRolesForUser(profile.UserName);
            ViewBag.ApiKeys = db.AccountApiKeys.Where(a => a.UserId == profile.UserId).ToList();
         //   ViewBag.Departments = db.Departments.ToList().OrderBy(p => p.DepartmentName);
          //  ViewBag.SelectedDepartments = profile.UserDepartments.Select(p => p.DepartmentId).ToList();
            return View(profile);
        }

        //
        // POST: /Role/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(UserProfile profile)
        {
            if (ModelState.IsValid)
            {
        //        profile = UpdateDepartments(profile);
                profile = UpdateRoles(profile);
                if (db.Entry(profile).State != EntityState.Modified)
                    db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        //
        // GET: /Role/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {
            UserProfile profile = db.UserProfiles.Find(id);
            if (profile == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(profile);
        }

        //
        // POST: /Role/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile profile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(profile);
            //Remove user in roles
            Roles.RemoveUserFromRoles(profile.UserName, Roles.GetRolesForUser(profile.UserName));
            //Remove user in admin
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private UserProfile UpdateRoles(UserProfile user)
        {
            string response = Request.Form["RolesOptions"];
            if (response != null)
            {
                string[] roles = response.Split(',');
                if (Roles.GetRolesForUser(user.UserName).Count() > 0)
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                foreach (string role in roles)
                {
                    Roles.AddUserToRole(user.UserName, role);
                }
            }
            return user;
        }

      

        #region Helpers
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}