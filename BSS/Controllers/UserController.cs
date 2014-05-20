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
    public class UserController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /User/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (BSSDbContext db = new BSSDbContext())
                {
                    if (db.Users == null)
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



                        bool userValid = db.Users.Any(user => user.UserName == username && user.Password == password);


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
                            ViewBag.UserTypes = db.UserTypes.ToList();
                            ModelState.AddModelError("", "Enter Valid UserName, Password and Select a Valid UserType");
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
            return View(db.Users.ToList());
          //  return View(db.Users.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            //ViewBag.UserTypes = db.UserTypes.ToList();
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //using (BookingStatusSystem db = new BookingStatusSystem())
            //{
            //    User usr = db.Users.FirstOrDefault(us => us.UserName.ToLower() == user.UserName.ToLower());

            //    // Check if user already exists
            //    if (usr == null)
            //    {
            //        // Insert name into the profile table
            //        db.Users.Add(new User { UserName = user.UserName });
            //        db.SaveChanges();
            //        return RedirectToAction("Index");

            //    }
            //    else
            //    {
            //        ModelState.AddModelError("UserName", "User Name already exists. Please enter a different User Name.");
            //    }
            //}

            return View(user);
        }


        public ActionResult Register()
        {
            //ViewBag.UserTypes = db.UserTypes.ToList();
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
           // ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeName");
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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