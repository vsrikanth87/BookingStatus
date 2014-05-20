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
    public class AgentDetailsController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /AgentDetails/

        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var agent = from s in db.AgentDetails
                        select s;
            switch (sortOrder)
            {
                case "Name_desc":
                    agent = agent.OrderByDescending(s => s.AgentId);
                    break;
                default:
                    agent = agent.OrderBy(s => s.AgentId);
                    break;
            }
            //page = 1;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(agent.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /AgentDetails/Details/5

        public ActionResult Details(int id = 0)
        {
            AgentDetails agentdetails = db.AgentDetails.Find(id);
            if (agentdetails == null)
            {
                return HttpNotFound();
            }
            return View(agentdetails);
        }

        //
        // GET: /AgentDetails/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AgentDetails/Create

        [HttpPost]
        public ActionResult Create(AgentDetails agentdetails)
        {
            //if (ModelState.IsValid)
            //{
            //    db.AgentDetails.Add(agentdetails);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(agentdetails);

            using (BSSDbContext db = new BSSDbContext())
            {
                AgentDetails Agent = db.AgentDetails.FirstOrDefault(a => a.AgentName.ToLower() == agentdetails.AgentName.ToLower());

                // Check if Agent already exists
                if (Agent == null)
                {
                    // Insert name into the profile table
                    db.AgentDetails.Add(new AgentDetails { AgentName = agentdetails.AgentName });
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("AgentName", "Agent name already exists. Please enter a different Agent name.");
                }
            }
            return View(agentdetails);
        }

        //
        // GET: /AgentDetails/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AgentDetails agentdetails = db.AgentDetails.Find(id);
            if (agentdetails == null)
            {
                return HttpNotFound();
            }
            return View(agentdetails);
        }

        //
        // POST: /AgentDetails/Edit/5

        [HttpPost]
        public ActionResult Edit(AgentDetails agentdetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentdetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentdetails);
        }

        //
        // GET: /AgentDetails/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AgentDetails agentdetails = db.AgentDetails.Find(id);
            if (agentdetails == null)
            {
                return HttpNotFound();
            }
            return View(agentdetails);
        }

        //
        // POST: /AgentDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentDetails agentdetails = db.AgentDetails.Find(id);
            db.AgentDetails.Remove(agentdetails);
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