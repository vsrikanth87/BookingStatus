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
    public class LoginController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /Login/


        public ActionResult Welcome()
        {
            return View();
        }


        public ActionResult Index()
        {
            return View(db.Logins.ToList());
        }

        //
        // GET: /Login/Details/5

        public ActionResult Details(int id = 0)
        {
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        //
        // GET: /Login/Create

        public ActionResult Create()
        {
            // ViewBag.ReturnUrl = returnUrl;
          //  ViewBag.UserTypes = db.UserTypes.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (BSSDbContext db = new BSSDbContext())
                {
                    if (db.Logins == null)
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



                        bool userValid = db.Logins.Any(user => user.UserName == username && user.Password == password);


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
                                return RedirectToAction("Index1", "BookingReceived");
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


        public ActionResult Create1()
        {
            // ViewBag.ReturnUrl = returnUrl;
            //  ViewBag.UserTypes = db.UserTypes.ToList();
            return View();
        }


        [HttpPost]
        public ActionResult Create1(Login login)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(login);

            //using (BSSDbContext db = new BSSDbContext())
            //{
            //    Login lg = db.Logins.FirstOrDefault(l => l.UserName.ToLower() == login.UserName.ToLower());

            //    // Check if Agent already exists
            //    if (lg == null)
            //    {
            //        // Insert name into the profile table
            //        db.Logins.Add(new Login { UserName = login.UserName });
            //        db.SaveChanges();
            //        return RedirectToAction("Index");

            //    }
            //    else
            //    {
            //        ModelState.AddModelError("UserName", "User name already exists. Please enter a different User name.");
            //    }
            //}
            //return View(login);
        }


        public ActionResult Register()
        {
            ViewBag.UserTypes = db.UserTypes.ToList();
            return View();
        }



        [HttpPost]
        public ActionResult Register(Login login)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Add(login);
                db.SaveChanges();
                // return RedirectToAction("Index");
            }

            return View(login);
        }


        public ActionResult Edit(int id = 0)
        {
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        //
        // POST: /Login/Edit/5

        [HttpPost]
        public ActionResult Edit(Login login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(login);
        }

        //
        // GET: /Login/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        //
        // POST: /Login/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Login login = db.Logins.Find(id);
            db.Logins.Remove(login);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }



        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #endregion


    }
}


