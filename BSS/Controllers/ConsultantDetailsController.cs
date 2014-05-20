using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSS.Models;
using PagedList;

namespace BSS.Controllers
{
    [Authorize]
    public class ConsultantDetailsController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /ConsultantDetails/

        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var consult = from s in db.ConsultantDetails
                          select s;
            switch (sortOrder)
            {
                case "Name_desc":
                    consult = consult.OrderByDescending(s => s.ConsultantId);
                    break;
                default:
                    consult = consult.OrderBy(s => s.ConsultantId);
                    break;
            }
            //page = 1;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(consult.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /ConsultantDetails/Details/5

        public ActionResult Details(int id = 0)
        {
            ConsultantDetails consultantdetails = db.ConsultantDetails.Find(id);
            if (consultantdetails == null)
            {
                return HttpNotFound();
            }
            return View(consultantdetails);
        }

        //
        // GET: /ConsultantDetails/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ConsultantDetails/Create

        [HttpPost]
        public ActionResult Create(ConsultantDetails consultantdetails)
        {
            //if (ModelState.IsValid)
            //{
            //    db.ConsultantDetails.Add(consultantdetails);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(consultantdetails);

            using (BSSDbContext db = new BSSDbContext())
            {
                ConsultantDetails consultant = db.ConsultantDetails.FirstOrDefault(c => c.ConsultantName.ToLower() == consultantdetails.ConsultantName.ToLower());

                // Check if consultant already exists
                if (consultant == null)
                {
                    // Insert name into the profile table
                    db.ConsultantDetails.Add(new ConsultantDetails { ConsultantName = consultantdetails.ConsultantName });
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("ConsultantName", "Consultant Name already exists. Please enter a different Consultant Name.");
                }
            }
            return View(consultantdetails);


        }

        //
        // GET: /ConsultantDetails/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ConsultantDetails consultantdetails = db.ConsultantDetails.Find(id);
            if (consultantdetails == null)
            {
                return HttpNotFound();
            }
            return View(consultantdetails);
        }

        //
        // POST: /ConsultantDetails/Edit/5

        [HttpPost]
        public ActionResult Edit(ConsultantDetails consultantdetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultantdetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consultantdetails);
        }

        //
        // GET: /ConsultantDetails/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ConsultantDetails consultantdetails = db.ConsultantDetails.Find(id);
            if (consultantdetails == null)
            {
                return HttpNotFound();
            }
            return View(consultantdetails);
        }

        //
        // POST: /ConsultantDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ConsultantDetails consultantdetails = db.ConsultantDetails.Find(id);
            db.ConsultantDetails.Remove(consultantdetails);
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