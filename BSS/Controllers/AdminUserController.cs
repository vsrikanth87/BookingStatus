using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BSS.Filters;
using BSS.Models;
using System.Data.Entity;
using System.Data;

namespace BSS.Controllers
{
    public class AdminUserController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /AdminUser/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminUser model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (BSSDbContext db = new BSSDbContext())
                {
                    if (db.AdminUsers == null)
                    {
                        if (ModelState.IsValid)
                        {

                            if (model.UserName == "Admin" && model.Password == "1234")
                            {
                                FormsAuthentication.SetAuthCookie(model.UserName, false);
                                return RedirectToAction("About", "Home");
                            }
                            {
                                ModelState.AddModelError("", "Invalid Username or Password");
                            }
                        }

                        return View(model);

                    }
                    else
                    {
                        string username = model.UserName;
                        string password = model.Password;



                        bool userValid = db.AdminUsers.Any(user => user.UserName == username && user.Password == password);


                        if (userValid)
                        {

                            FormsAuthentication.SetAuthCookie(username, false);
                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("About", "Home");
                            }
                        }
                        else
                        {
                           // ViewBag.UserTypes = db.UserTypes.ToList();
                            ModelState.AddModelError("", "Enter Valid UserName and Password");
                        }
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Index()
        {
            return View(db.AdminUsers.ToList());
        }

        //
        // GET: /AdminUser/Details/5

        public ActionResult Details(int id = 0)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            if (adminuser == null)
            {
                return HttpNotFound();
            }
            return View(adminuser);
        }

        //
        // GET: /AdminUser/Create

        public ActionResult Create()
        {
            AdminUser adm = new AdminUser();
            return View(adm);
        }

        //
        // POST: /AdminUser/Create

        [HttpPost]
        public ActionResult Create(AdminUser adminuser)
        {
            if (ModelState.IsValid)
            {
                db.AdminUsers.Add(adminuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminuser);

            //using (BSSDbContext db = new BSSDbContext())
            //{
            // AdminUser Adm = db.AdminUsers.FirstOrDefault(ad => ad.UserName.ToLower() == adminuser.UserName.ToLower());

            //    // Check if Agent already exists
            //    if (Adm == null)
            //    {
            //        // Insert name into the profile table
            //        db.AdminUsers.Add(new AdminUser { UserName = adminuser.UserName });
            //        db.SaveChanges();
            //        return RedirectToAction("Index");

            //    }
            //    else
            //    {
            //        ModelState.AddModelError("UserName", "User name already exists. Please enter a different User name.");
            //    }
            //}
            //return View(adminuser);

        }

        //
        // GET: /AdminUser/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            if (adminuser == null)
            {
                return HttpNotFound();
            }
            return View(adminuser);
        }

        //
        // POST: /AdminUser/Edit/5

        [HttpPost]
        public ActionResult Edit(AdminUser adminuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminuser);
        }

        //
        // GET: /AdminUser/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            if (adminuser == null)
            {
                return HttpNotFound();
            }
            return View(adminuser);
        }

        //
        // POST: /AdminUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            db.AdminUsers.Remove(adminuser);
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