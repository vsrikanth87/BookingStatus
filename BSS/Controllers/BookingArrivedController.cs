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
using System.IO;
using Microsoft.Reporting.WebForms;
using PagedList;
using System.Web.Mvc.Ajax;


namespace BSS.Controllers
{
    public class BookingArrivedController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /BookingArrived/

        public ViewResult Index1(string searchString)
        {
            // IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-US", true);
            searchString = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            // DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
            var value10 = from bar in db.BookingArriveds
                          where (bar.BookingArrivedEnquiredDateTime.Contains(searchString)) // == searchString
                          select bar;
            return View(value10);

        }

        //public ViewResult Pending(string searchString, string searchString1, string searchString2)
        //{

        //    searchString = DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy");
        //    searchString1 = DateTime.Now.ToString("dd'/'MM'/'yyyy");
        //    searchString2 = "--";
        //    var data1 = db.BookingArriveds.Where(x => x.BookingStatus == searchString2
        //                    && x.BookingArrivedEnquiredDateTime == searchString
        //                    || x.BookingStatus == searchString2
        //                    && x.BookingArrivedEnquiredDateTime == searchString1).ToList();
        //    return View(data1);

        //}

        //public ViewResult Pending(string searchString)
        //{

        //    searchString = "--";
        //    // DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
        //    var value10 = from bar in db.BookingArriveds
        //                  where (bar.BookingStatus.Contains(searchString)) // == searchString
        //                  select bar;
        //    return View(value10);

        //}

        public ViewResult Pending(string searchString, string searchString1, string searchString2)
        {

            searchString = DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy");
            searchString1 = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            searchString2 = "--";
            var data1 = db.BookingArriveds.Where(x => x.BookingStatus == searchString2
                            && x.BookingArrivedEnquiredDateTime == searchString
                            || x.BookingStatus == searchString2
                            && x.BookingArrivedEnquiredDateTime != searchString1).ToList();
            return View(data1);

        }


        [HttpGet]
        public ActionResult Report(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var rp = from s in db.BookingArriveds
                     select s;
            switch (sortOrder)
            {
                case "Name_desc":
                    rp = rp.OrderByDescending(s => s.BookingArrivedId);
                    break;
                default:
                    rp = rp.OrderBy(s => s.BookingArrivedId);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(rp.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult BookingArrived()
        {
            using (BSSDbContext db = new BSSDbContext())
            {
                var v = db.BookingArriveds.ToList();
                return View(v);
            }
        }

        public ActionResult Report1(string id)
        {
            //LocalReport lr = new LocalReport();
            //string path = Path.Combine(Server.MapPath("~/Report"), "ReportBookingArrived.rdlc");
            //if (System.IO.File.Exists(path))
            //{
            //    lr.ReportPath = path;
            //}
            //else
            //{
            //    return View("Index1");
            //}
            //List<BookingArrived> cm = new List<BookingArrived>();
            //using (BSSDbContext dc = new BSSDbContext())
            //{

            //    cm = dc.BookingArriveds.ToList();

            //}
            //ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            //lr.DataSources.Add(rd);
            //string reportType = id;
            //string mimeType;
            //string encoding;
            //string fileNameExtension;



            //string deviceInfo =

            //"<DeviceInfo>" +
            //"  <OutputFormat>" + id + "</OutputFormat>" +
            //"  <PageWidth>17in</PageWidth>" +
            //"  <PageHeight>10in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>1.2in</MarginLeft>" +
            //"  <MarginRight>0.2in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

            //Warning[] warnings;
            //string[] streams;
            //byte[] renderedBytes;

            //renderedBytes = lr.Render(
            //    reportType,
            //    deviceInfo,
            //    out mimeType,
            //    out encoding,
            //    out fileNameExtension,
            //    out streams,
            //    out warnings);


            //return File(renderedBytes, mimeType);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                               join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                               join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                               join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                               select new
                                               {
                                                   ba.BookingArrivedId,
                                                   ba.BookingArrivedEnquiredDateTime,
                                                   ba.BookingArrivedEnquiredTime,
                                                   ba.BookingArrivedEnquiredDetails,
                                                   ba.TravelDate,
                                                   ba.AgentDetail.AgentName,
                                                   ba.ConsultantDetail.ConsultantName,
                                                   ba.QuotationSendDateTime,
                                                   ba.QuotationSendTime,
                                                   ba.TourPlanRefrence,
                                                   ba.HotelDetail.HotelName,
                                                   ba.BookingStatus,
                                                   ba.ConfirmationDate
                                               };


            reportDataSource.Value = BookingArrivedDatefilterList.ToList();

            localReport.DataSources.Add(reportDataSource);

            string reportType = id;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + id + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }





        public ActionResult Report2(string bookingarriveddate, string id)
        {

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "ReportBookingArrived.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index1");
            }
            IList<BookingArrived> cm = new List<BookingArrived>();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", cm);
            // reportDataSource.Name = "DataSet1";
            if (bookingarriveddate != null)
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarriveddate
                                                   select ba;


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else
                reportDataSource.Value = cm;

            lr.DataSources.Add(reportDataSource);

            string reportType = id;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>16in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }



        public ActionResult Details(int id = 0)
        {
            BookingArrived bookingarrived = db.BookingArriveds.Find(id);
            if (bookingarrived == null)
            {
                return HttpNotFound();
            }
            return View(bookingarrived);
        }

        //
        // GET: /BookingArrived/Create

        public ActionResult Create()
        {
            ViewBag.HotelDetails = db.HotelDetails.ToList();
            ViewBag.AgentDetails = db.AgentDetails.ToList();
            ViewBag.ConsultantDetails = db.ConsultantDetails.ToList();
            return View();
        }


        //
        // POST: /BookingArrived/Create

        [HttpPost]
        public ActionResult Create(BookingArrived bookingarrived)
        {
            if (ModelState.IsValid)
            {
                db.BookingArriveds.Add(bookingarrived);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookingarrived);
        }

        //
        // GET: /BookingArrived/Edit/5

        public ActionResult Edit(int id = 0)
        {


            BookingArrived bookingarrived = db.BookingArriveds.Find(id);
            if (bookingarrived == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.HotelDetails, "HotelId", "HotelName", bookingarrived.HotelId);
            ViewBag.AgentId = new SelectList(db.AgentDetails, "AgentId", "AgentName", bookingarrived.AgentId);
            ViewBag.ConsultantId = new SelectList(db.ConsultantDetails, "ConsultantId", "ConsultantName", bookingarrived.ConsultantId);
            return View(bookingarrived);
        }

        //
        // POST: /BookingArrived/Edit/5

        [HttpPost]
        public ActionResult Edit(BookingArrived bookingarrived)
        {
            if (ModelState.IsValid)
            {

                db.Entry(bookingarrived).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookingarrived);
        }

        //
        // GET: /BookingArrived/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BookingArrived bookingarrived = db.BookingArriveds.Find(id);
            if (bookingarrived == null)
            {
                return HttpNotFound();
            }
            return View(bookingarrived);
        }

        //
        // POST: /BookingArrived/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingArrived bookingarrived = db.BookingArriveds.Find(id);
            db.BookingArriveds.Remove(bookingarrived);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ViewResult Index(string searchString, string searchString1, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.HotleSortParm = sortOrder == "Hotle" ? "Hotle_desc" : "Hotle";
            ViewBag.ConsultentSortParm = sortOrder == "Consultent" ? "Consultent_desc" : "Consultent";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (searchString == null)
            {

                searchString = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                // DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
                var value = from bar in db.BookingArriveds
                            where (bar.BookingArrivedEnquiredDateTime.Contains(searchString)) // == searchString
                            select bar;
                switch (sortOrder)
                {
                    case "Name_desc":
                        value = value.OrderByDescending(s => s.AgentId);
                        break;
                    case "Hotle":
                        value = value.OrderBy(s => s.HotelId);
                        break;
                    case "Hotle_desc":
                        value = value.OrderByDescending(s => s.HotelId);
                        break;
                    case "Consultent":
                        value = value.OrderBy(s => s.ConsultantId);
                        break;
                    case "Consultent_desc":
                        value = value.OrderByDescending(s => s.ConsultantId);
                        break;
                    default:
                        value = value.OrderBy(s => s.AgentId);
                        break;
                }

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(value.ToPagedList(pageNumber, pageSize));
            }
            else
            {

                var value = from m in db.BookingArriveds
                            select m;
                value = value.Where(ba => ba.BookingArrivedEnquiredDateTime.Contains(searchString));
                switch (sortOrder)
                {
                    case "Name_desc":
                        value = value.OrderByDescending(s => s.AgentId);
                        break;
                    case "Hotle":
                        value = value.OrderBy(s => s.HotelId);
                        break;
                    case "Hotle_desc":
                        value = value.OrderByDescending(s => s.HotelId);
                        break;
                    case "Consultent":
                        value = value.OrderBy(s => s.ConsultantId);
                        break;
                    case "Consultent_desc":
                        value = value.OrderByDescending(s => s.ConsultantId);
                        break;
                    default:
                        value = value.OrderBy(s => s.AgentId);
                        break;
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(value.ToPagedList(pageNumber, pageSize));
            }


        }
        //
        // GET: /BookingArrived/Details/5

        //public ViewResult Report(string SearchBy, string searchString)
        //{
        //    var value1 = from m in db.BookingArriveds
        //                 select m;


        //    var query = from b in db.BookingArriveds
        //                join a in db.AgentDetails on b.AgentId equals a.AgentId
        //                select b;

        //    var query1 = from b in db.BookingArriveds
        //                 join c in db.ConsultantDetails on b.ConsultantId equals c.ConsultantId
        //                 select b;

        //    var query2 = from b in db.BookingArriveds
        //                 join h in db.HotelDetails on b.HotelId equals h.HotelId
        //                 select b;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {

        //        if (SearchBy == "BookingStatus")
        //        {
        //            value1 = value1.Where(s => s.BookingStatus.Contains(searchString));
        //        }

        //        else if (SearchBy == "BookingArrivedEnquiredDetails")
        //        {
        //            value1 = value1.Where(s => s.BookingArrivedEnquiredDetails.Contains(searchString));
        //        }

        //        else if (SearchBy == "AgentName")
        //        {

        //            query = query.Where(s => s.AgentDetail.AgentName.Contains(searchString));
        //            return View(query.ToList());
        //        }

        //        else if (SearchBy == "ConsultantName")
        //        {
        //            query1 = query1.Where(s => s.ConsultantDetail.ConsultantName.Contains(searchString));
        //            return View(query1.ToList());
        //        }

        //        else if (SearchBy == "HotelName")
        //        {
        //            query2 = query2.Where(s => s.HotelDetail.HotelName.Contains(searchString));
        //            return View(query2.ToList());
        //        }

        //        else if (SearchBy == "BookingArrivedEnquiredDateTime")
        //        {

        //            value1 = value1.Where(ba => ba.BookingArrivedEnquiredDateTime.Contains(searchString));
        //        }

        //        else if (SearchBy == "TravelDate")
        //        {

        //            value1 = value1.Where(ta => ta.TravelDate.Contains(searchString));

        //        }



        //        else if (SearchBy == "QuotationSendDateTime")
        //        {

        //            value1 = value1.Where(s => s.QuotationSendDateTime.Contains(searchString));

        //        }

        //        else if (SearchBy == "ConfirmationDate")
        //        {

        //            value1 = value1.Where(s => s.ConfirmationDate.Contains(searchString));
        //        }
        //        else
        //        {
        //            return View(value1);
        //        }
        //    }


        //    return View(value1);

        //}
        public ViewResult Report(string SearchBy, string searchString, string sortOrder, string currentFilter, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var value1 = from m in db.BookingArriveds
                         select m;


            var query = from b in db.BookingArriveds
                        join a in db.AgentDetails on b.AgentId equals a.AgentId
                        select b;

            var query1 = from b in db.BookingArriveds
                         join c in db.ConsultantDetails on b.ConsultantId equals c.ConsultantId
                         select b;

            var query2 = from b in db.BookingArriveds
                         join h in db.HotelDetails on b.HotelId equals h.HotelId
                         select b;
            switch (sortOrder)
            {
                case "Name_desc":
                    value1 = value1.OrderByDescending(s => s.BookingArrivedId);
                    break;
                default:
                    value1 = value1.OrderBy(s => s.BookingArrivedId);
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {

                if (SearchBy == "BookingStatus")
                {
                    value1 = value1.Where(s => s.BookingStatus.Contains(searchString));
                }

                else if (SearchBy == "BookingArrivedEnquiredDetails")
                {
                    value1 = value1.Where(s => s.BookingArrivedEnquiredDetails.Contains(searchString));
                }

                else if (SearchBy == "AgentName")
                {

                    value1 = value1.Where(s => s.AgentDetail.AgentName.Contains(searchString));
                    return View(value1.ToPagedList(pageNumber, pageSize));
                }

                else if (SearchBy == "ConsultantName")
                {
                    value1 = value1.Where(s => s.ConsultantDetail.ConsultantName.Contains(searchString));
                    return View(value1.ToPagedList(pageNumber, pageSize));
                }

                else if (SearchBy == "HotelName")
                {
                    value1 = value1.Where(s => s.HotelDetail.HotelName.Contains(searchString));
                    return View(value1.ToPagedList(pageNumber, pageSize));

                }

                else if (SearchBy == "BookingArrivedEnquiredDateTime")
                {

                    value1 = value1.Where(ba => ba.BookingArrivedEnquiredDateTime.Contains(searchString));
                }

                else if (SearchBy == "TravelDate")
                {

                    value1 = value1.Where(ta => ta.TravelDate.Contains(searchString));

                }

                else if (SearchBy == "QuotationSendDateTime")
                {

                    value1 = value1.Where(s => s.QuotationSendDateTime.Contains(searchString));

                }

                else if (SearchBy == "ConfirmationDate")
                {

                    value1 = value1.Where(s => s.ConfirmationDate.Contains(searchString));
                }
                else
                {
                    return View(value1);

                }
            }

            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            return View(value1.ToPagedList(pageNumber, pageSize));

        }


        public ActionResult displayreport(string bookingarriveddate, string id)
        {
            return View();
        }


        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Create", "Login");
        }
        #endregion



        [AllowAnonymous]
        public ActionResult ReportViewer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewer(SearchParameterModel um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReport(string bookingarrivedrequestdatetime, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if (bookingarrivedrequestdatetime != null)
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>12in</PageWidth>" +
                //"  <PageHeight>1in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.1in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0.1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

        public ActionResult DownloadReport(string bookingarrivedrequestdatetime, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if (bookingarrivedrequestdatetime != null)
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + format + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }


        [AllowAnonymous]
        public ActionResult ReportViewer1(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewer1(SearchParameterModel um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateBookingStatus(string bookingarrivedrequestdatetime, string bookingstatus, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (bookingstatus != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.BookingStatus) == bookingstatus
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>12in</PageWidth>" +
                //"  <PageHeight>1in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.1in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0.1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

        public ActionResult DownloadReportDateBookingStatus(string bookingarrivedrequestdatetime, string bookingstatus, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (bookingstatus != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.BookingStatus) == bookingstatus
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };

                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + format + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }


        [AllowAnonymous]
        public ActionResult ReportViewerDateAgentWise(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewerDateAgentWise(SearchParameterModel um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateAgent(string bookingarrivedrequestdatetime, string agent, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (agent != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.AgentDetail.AgentName) == agent
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>12in</PageWidth>" +
                //"  <PageHeight>1in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.1in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0.1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

        public ActionResult DownloadReportDateAgent(string bookingarrivedrequestdatetime, string agent, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (agent != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.AgentDetail.AgentName) == agent
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + format + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }


        //===========

        [AllowAnonymous]
        public ActionResult ReportViewerDateConsultantWise(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewerDateConsultantWise(SearchParameterModel um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateConsultant(string bookingarrivedrequestdatetime, string consultant, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (consultant != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.ConsultantDetail.ConsultantName) == consultant
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>12in</PageWidth>" +
                //"  <PageHeight>1in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.1in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0.1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

        public ActionResult DownloadReportDateConsultant(string bookingarrivedrequestdatetime, string consultant, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (consultant != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.ConsultantDetail.ConsultantName) == consultant
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + format + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }


        //==================

        [AllowAnonymous]
        public ActionResult ReportViewerDateHotelWise(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewerDateHotelWise(SearchParameterModel um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateHotel(string bookingarrivedrequestdatetime, string hotel, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (hotel != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.HotelDetail.HotelName) == hotel
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>12in</PageWidth>" +
                //"  <PageHeight>1in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.1in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0.1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

        public ActionResult DownloadReportDateHotel(string bookingarrivedrequestdatetime, string hotel, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/BookingArrivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (hotel != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                                                   (ba.HotelDetail.HotelName) == hotel
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + format + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }

        //=============

        [AllowAnonymous]
        public ActionResult ReportViewerInBetweenDate(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult ReportViewerInBetweenDate(SearchParameterModel um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportInBetweenDate(string bookingarrivedrequestdatetime, string bookingarrivedrequestdatetime1, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/ReportBookingArrived.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (bookingarrivedrequestdatetime1 != null))
            {

                ////IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-US", true);
                ////DateTime BookingArrivedEnquiredDateTime11 = DateTime.Parse(bookingarrivedrequestdatetime, theCultureInfo);
                ////DateTime BookingArrivedEnquiredDateTime12 = DateTime.Parse(bookingarrivedrequestdatetime1, theCultureInfo);

                ////var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                ////                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime11 &&
                ////                                   (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime12
                ////                                   select ba;
                ////reportDataSource.Value = BookingArrivedDatefilterList;

                var memberl = from ba in db.BookingArriveds
                              where ba.BookingArrivedEnquiredDateTime.CompareTo(bookingarrivedrequestdatetime1) >= 0 &&
                                    ba.BookingArrivedEnquiredDateTime.CompareTo(bookingarrivedrequestdatetime) <= 0
                              select new
                              {
                                  ba.BookingArrivedId,
                                  ba.BookingArrivedEnquiredDateTime,
                                  ba.BookingArrivedEnquiredTime,
                                  ba.BookingArrivedEnquiredDetails,
                                  ba.TravelDate,
                                  ba.AgentDetail.AgentName,
                                  ba.ConsultantDetail.ConsultantName,
                                  ba.QuotationSendDateTime,
                                  ba.QuotationSendTime,
                                  ba.TourPlanRefrence,
                                  ba.HotelDetail.HotelName,
                                  ba.BookingStatus,
                                  ba.ConfirmationDate
                              };


                reportDataSource.Value = memberl;

                //var BookingArrivedDatefilterList = db.BookingArriveds.Where(x => DateTime.Parse(x.BookingArrivedEnquiredDateTime) >= DateTime.Parse(bookingarrivedrequestdatetime)
                //               && DateTime.Parse(x.BookingArrivedEnquiredDateTime) <= DateTime.Parse(bookingarrivedrequestdatetime1)).ToList();
                //reportDataSource.Value = BookingArrivedDatefilterList;
               
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>12in</PageWidth>" +
                //"  <PageHeight>1in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>0.1in</MarginLeft>" +
                 "  <MarginRight>0.1in</MarginRight>" +
                 "  <MarginBottom>0.1in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            if (format == null)
            {
                return File(renderedBytes, "image/jpeg");
            }
            else if (format == "PDF")
            {
                return File(renderedBytes, "pdf");
            }
            else
            {
                return File(renderedBytes, "image/jpeg");
            }
        }

        public ActionResult DownloadReportInBetweenDate(string bookingarrivedrequestdatetime, string bookingarrivedrequestdatetime1, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/ReportBookingArrived.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            if ((bookingarrivedrequestdatetime != null) || (bookingarrivedrequestdatetime1 != null))
            {
                //var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                //                                   join h in db.HotelDetails on ba.ConsultantId equals h.HotelId
                //                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
                //                                   (ba.HotelDetail.HotelName) == hotel
                //                                   select ba;

                var BookingArrivedDatefilterList = db.BookingArriveds.Where(x => DateTime.Parse(x.BookingArrivedEnquiredDateTime) >= DateTime.Parse(bookingarrivedrequestdatetime)
                              && DateTime.Parse(x.BookingArrivedEnquiredDateTime) <= DateTime.Parse(bookingarrivedrequestdatetime1)).ToList();
                reportDataSource.Value = BookingArrivedDatefilterList;
            }
            else

                reportDataSource.Value = db.BookingArriveds;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            // string Parameter = bookingarriveddate;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

               "<DeviceInfo>" +
               "  <OutputFormat>" + format + "</OutputFormat>" +
               "  <PageWidth>17in</PageWidth>" +
               "  <PageHeight>10in</PageHeight>" +
               "  <MarginTop>0.5in</MarginTop>" +
               "  <MarginLeft>1.2in</MarginLeft>" +
               "  <MarginRight>0.2in</MarginRight>" +
               "  <MarginBottom>0.5in</MarginBottom>" +
               "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }


    }
}