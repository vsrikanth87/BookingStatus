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
    public class HotelDetailsController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /HotelDetails/

        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var hotel = from s in db.HotelDetails
                        select s;
            switch (sortOrder)
            {
                case "Name_desc":
                    hotel = hotel.OrderByDescending(s => s.HotelId);
                    break;
                default:
                    hotel = hotel.OrderBy(s => s.HotelId);
                    break;
            }
           // page = 1;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(hotel.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /HotelDetails/Details/5

        public ActionResult Details(int id = 0)
        {
            HotelDetails hoteldetails = db.HotelDetails.Find(id);
            if (hoteldetails == null)
            {
                return HttpNotFound();
            }
            return View(hoteldetails);
        }

        //
        // GET: /HotelDetails/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HotelDetails/Create

        [HttpPost]
        public ActionResult Create(HotelDetails hoteldetails)
        {
            //if (ModelState.IsValid)
            //{
            //    db.HotelDetails.Add(hoteldetails);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(hoteldetails);

            using (BSSDbContext db = new BSSDbContext())
            {
                HotelDetails Hotel = db.HotelDetails.FirstOrDefault(h => h.HotelName.ToLower() == hoteldetails.HotelName.ToLower());

                // Check if Hotel already exists
                if (Hotel == null)
                {
                    // Insert name into the profile table
                    db.HotelDetails.Add(new HotelDetails { HotelName = hoteldetails.HotelName });
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("HotelName", "Hotel Name already exists. Please enter a different Hotel Name.");
                }
            }
            return View(hoteldetails);
        }

        //
        // GET: /HotelDetails/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HotelDetails hoteldetails = db.HotelDetails.Find(id);
            if (hoteldetails == null)
            {
                return HttpNotFound();
            }
            return View(hoteldetails);
        }

        //
        // POST: /HotelDetails/Edit/5

        [HttpPost]
        public ActionResult Edit(HotelDetails hoteldetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoteldetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hoteldetails);
        }

        //
        // GET: /HotelDetails/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HotelDetails hoteldetails = db.HotelDetails.Find(id);
            if (hoteldetails == null)
            {
                return HttpNotFound();
            }
            return View(hoteldetails);
        }

        //
        // POST: /HotelDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            HotelDetails hoteldetails = db.HotelDetails.Find(id);
            db.HotelDetails.Remove(hoteldetails);
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