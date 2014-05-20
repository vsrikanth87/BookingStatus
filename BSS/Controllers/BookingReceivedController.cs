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
    public class BookingReceivedController : Controller
    {
        private BSSDbContext db = new BSSDbContext();

        //
        // GET: /BookingReceived/

        public ViewResult Index1(string searchString)
        {
            searchString = DateTime.Now.ToString("dd/MM/yyyy");
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
            var value3 = from ba in db.BookingReceiveds
                         where (ba.BookingArrivedEnquiredDateTime) == BookingArrivedEnquiredDateTime1
                         select ba;
            return View(value3);

        }



        public ViewResult Pending(string searchString, string searchString1, string searchString2)
        {
            searchString = DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy");
            searchString1 = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(searchString1, theCultureInfo);
            searchString2 = "--";
            var data1 = db.BookingReceiveds.Where(x => x.BookingStatus == searchString2
                            && x.BookingArrivedEnquiredDateTime == BookingArrivedEnquiredDateTime1
                            || x.BookingStatus == searchString2
                            && x.BookingArrivedEnquiredDateTime != BookingArrivedEnquiredDateTime2).ToList();
            return View(data1.OrderBy(ab => ab.BookingArrivedEnquiredDateTime));
        }



        [HttpGet]

        public ActionResult Report(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var rp = from s in db.BookingReceiveds
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

        //public ActionResult BookingArrived()
        //{
        //    using (BSSDbContext db = new BSSDbContext())
        //    {
        //        var v = db.BookingArriveds.ToList();
        //        return View(v);
        //    }
        //}

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
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
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
                                                   ba.BookingType,
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


        public ActionResult Details(int id = 0)
        {
            BookingReceived bookingreceived = db.BookingReceiveds.Find(id);
            if (bookingreceived == null)
            {
                return HttpNotFound();
            }
            return View(bookingreceived);
        }

        public ActionResult Create()
        {
            ViewBag.HotelDetails = db.HotelDetails.ToList();
            ViewBag.AgentDetails = db.AgentDetails.ToList();
            ViewBag.ConsultantDetails = db.ConsultantDetails.ToList();
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(BookingReceived bookingreceived)
        {
            if (ModelState.IsValid)
            {
                db.BookingReceiveds.Add(bookingreceived);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelDetails = db.HotelDetails.ToList();
            ViewBag.AgentDetails = db.AgentDetails.ToList();
            ViewBag.ConsultantDetails = db.ConsultantDetails.ToList();

            return View(bookingreceived);
        }

        public ActionResult Edit(int id = 0)
        {


            BookingReceived bookingreceived = db.BookingReceiveds.Find(id);
            if (bookingreceived == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.HotelDetails, "HotelId", "HotelName", bookingreceived.HotelId);
            ViewBag.AgentId = new SelectList(db.AgentDetails, "AgentId", "AgentName", bookingreceived.AgentId);
            ViewBag.ConsultantId = new SelectList(db.ConsultantDetails, "ConsultantId", "ConsultantName", bookingreceived.ConsultantId);
            return View(bookingreceived);
        }

        [HttpPost]
        public ActionResult Edit(BookingReceived bookingreceived)
        {
            if (ModelState.IsValid)
            {

                db.Entry(bookingreceived).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookingreceived);
        }

        public ActionResult Delete(int id = 0)
        {
            BookingReceived bookingreceived = db.BookingReceiveds.Find(id);
            if (bookingreceived == null)
            {
                return HttpNotFound();
            }
            return View(bookingreceived);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingReceived bookingreceived = db.BookingReceiveds.Find(id);
            db.BookingReceiveds.Remove(bookingreceived);
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

            if ((searchString == null) || (searchString == ""))
            {

                searchString = DateTime.Now.ToString("dd/MM/yyyy");
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
                var value = from ba in db.BookingReceiveds
                            where (ba.BookingArrivedEnquiredDateTime) == BookingArrivedEnquiredDateTime1
                            select ba;
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

                var value = from m in db.BookingReceiveds
                            select m;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
                value = value.Where(ba => ba.BookingArrivedEnquiredDateTime == BookingArrivedEnquiredDateTime1);
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

            var value1 = from m in db.BookingReceiveds
                         select m;


            var query = from b in db.BookingReceiveds
                        join a in db.AgentDetails on b.AgentId equals a.AgentId
                        select b;

            var query1 = from b in db.BookingReceiveds
                         join c in db.ConsultantDetails on b.ConsultantId equals c.ConsultantId
                         select b;

            var query2 = from b in db.BookingReceiveds
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
            if (searchString != null)
            {
                if (SearchBy == "BookingStatus")
                {
                    value1 = value1.Where(s => s.BookingType.Contains(searchString));
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

                    ViewBag.CurrentFilter = searchString;
                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(searchString, theCultureInfo);
                    value1 = value1.Where(s => s.BookingArrivedEnquiredDateTime == BookingArrivedEnquiredDateTime1);
                }

                else if (SearchBy == "TravelDate")
                {

                    ViewBag.CurrentFilter = searchString;
                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime TravelDate1 = DateTime.Parse(searchString, theCultureInfo);
                    value1 = value1.Where(s => s.TravelDate == TravelDate1);

                }

                else if (SearchBy == "QuotationSendDateTime")
                {

                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime QuotationSendDateTime1 = DateTime.Parse(searchString, theCultureInfo);
                    value1 = value1.Where(s => s.QuotationSendDateTime == QuotationSendDateTime1);

                }

                else if (SearchBy == "ConfirmationDate")
                {
                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime ConfirmationDate1 = DateTime.Parse(searchString, theCultureInfo);
                    value1 = value1.Where(s => s.ConfirmationDate == ConfirmationDate1);
                }
                else
                {
                    return View(value1);

                }
            }

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
        public ViewResult ReportViewer(SearchParameterModel1 um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReport(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string format)
        {


            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null))
            {
                //var BookingArrivedDatefilterList = from ba in db.BookingArriveds
                //                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                //                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2 
                //                                   select ba;

                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };
                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-GB/library/ms155397.aspx            
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

        public ActionResult DownloadReport(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   select new
                                                   {
                                                       ba.BookingArrivedId,
                                                       ba.BookingArrivedEnquiredDateTime,
                                                       ba.BookingArrivedEnquiredTime,
                                                       ba.BookingArrivedEnquiredDetails,
                                                       ba.TravelDate,
                                                       ba.AgentDetail.AgentName,
                                                       ba.ConsultantDetail.ConsultantName,
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };
                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

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
        public ViewResult ReportViewer1(SearchParameterModel1 um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateBookingStatus(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string bookingstatus, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (bookingstatus != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-GB/library/ms155397.aspx            
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

        public ActionResult DownloadReportDateBookingStatus(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string bookingstatus, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (bookingstatus != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };

                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

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
        public ViewResult ReportViewerDateAgentWise(SearchParameterModel1 um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateAgent(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string agent, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (agent != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-GB/library/ms155397.aspx            
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

        public ActionResult DownloadReportDateAgent(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string agent, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (agent != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

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
        public ViewResult ReportViewerDateConsultantWise(SearchParameterModel1 um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateConsultant(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string consultant, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (consultant != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-GB/library/ms155397.aspx            
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

        public ActionResult DownloadReportDateConsultant(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string consultant, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (consultant != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

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
        public ViewResult ReportViewerDateHotelWise(SearchParameterModel1 um)
        {
            return View(um);
        }

        public FileContentResult GenerateAndDisplayReportDateHotel(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string hotel, string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (hotel != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-GB/library/ms155397.aspx            
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

        public ActionResult DownloadReportDateHotel(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string hotel, string format)
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/BookingReceivedReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
            ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
            DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
            DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
            if ((BookingArrivedEnquiredDateTime1 != null) && (BookingArrivedEnquiredDateTime2 != null) || (hotel != null))
            {
                var BookingArrivedDatefilterList = from ba in db.BookingReceiveds
                                                   join ag in db.AgentDetails on ba.AgentId equals ag.AgentId
                                                   join cn in db.ConsultantDetails on ba.ConsultantId equals cn.ConsultantId
                                                   join ht in db.HotelDetails on ba.HotelId equals ht.HotelId
                                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime1
                                                   && (ba.BookingArrivedEnquiredDateTime) <= BookingArrivedEnquiredDateTime2
                                                   &&
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
                                                       ba.BookingType,
                                                       ba.QuotationSendDateTime,
                                                       ba.QuotationSendTime,
                                                       ba.TourPlanRefrence,
                                                       ba.HotelDetail.HotelName,
                                                       ba.BookingStatus,
                                                       ba.ConfirmationDate
                                                   };


                reportDataSource.Value = BookingArrivedDatefilterList.ToList();
            }
            else

                reportDataSource.Value = db.BookingReceiveds;

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

        //[AllowAnonymous]
        //public ActionResult ReportViewerInBetweenDate(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ViewResult ReportViewerInBetweenDate(SearchParameterModel um)
        //{
        //    return View(um);
        //}

        ////public FileContentResult GenerateAndDisplayReportInBetweenDate(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string bookingarrivedrequestdatetime1, string format)
        ////{
        ////    LocalReport localReport = new LocalReport();
        ////    localReport.ReportPath = Server.MapPath("~/Report/ReportBookingArrived.rdlc");
        ////    ReportDataSource reportDataSource = new ReportDataSource();
        ////    reportDataSource.Name = "DataSet1";
        ////    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        ////    ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
        ////    ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
        ////    DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
        ////    DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
        ////    if ((bookingarrivedrequestdatetime != null) || (bookingarrivedrequestdatetime1 != null))
        ////    {

        ////        ////IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        ////        ////DateTime BookingArrivedEnquiredDateTime11 = DateTime.Parse(bookingarrivedrequestdatetime, theCultureInfo);
        ////        ////DateTime BookingArrivedEnquiredDateTime12 = DateTime.Parse(bookingarrivedrequestdatetime1, theCultureInfo);

        ////        ////var BookingArrivedDatefilterList = from ba in db.BookingArriveds
        ////        ////                                   where (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime11 &&
        ////        ////                                   (ba.BookingArrivedEnquiredDateTime) >= BookingArrivedEnquiredDateTime12
        ////        ////                                   select ba;
        ////        ////reportDataSource.Value = BookingArrivedDatefilterList;

        ////        var memberl = from ba in db.BookingArriveds
        ////                      where ba.BookingArrivedEnquiredDateTime.CompareTo(bookingarrivedrequestdatetime1) >= 0 &&
        ////                            ba.BookingArrivedEnquiredDateTime.CompareTo(bookingarrivedrequestdatetime) <= 0
        ////                      select ba;


        ////        reportDataSource.Value = memberl;

        ////        //var BookingArrivedDatefilterList = db.BookingArriveds.Where(x => DateTime.Parse(x.BookingArrivedEnquiredDateTime) >= DateTime.Parse(bookingarrivedrequestdatetime)
        ////        //               && DateTime.Parse(x.BookingArrivedEnquiredDateTime) <= DateTime.Parse(bookingarrivedrequestdatetime1)).ToList();
        ////        //reportDataSource.Value = BookingArrivedDatefilterList;

        ////    }
        ////    else

        ////        reportDataSource.Value = db.BookingArriveds;

        ////    localReport.DataSources.Add(reportDataSource);
        ////    string reportType = "Image";
        ////    string mimeType;
        ////    string encoding;
        ////    string fileNameExtension;
        ////    //The DeviceInfo settings should be changed based on the reportType            
        ////    //http://msdn2.microsoft.com/en-GB/library/ms155397.aspx            
        ////    string deviceInfo = "<DeviceInfo>" +
        ////         "  <OutputFormat>jpeg</OutputFormat>" +
        ////         "  <PageWidth>12in</PageWidth>" +
        ////        //"  <PageHeight>1in</PageHeight>" +
        ////         "  <MarginTop>0.5in</MarginTop>" +
        ////         "  <MarginLeft>0.1in</MarginLeft>" +
        ////         "  <MarginRight>0.1in</MarginRight>" +
        ////         "  <MarginBottom>0.1in</MarginBottom>" +
        ////         "</DeviceInfo>";
        ////    Warning[] warnings;
        ////    string[] streams;
        ////    byte[] renderedBytes;
        ////    //Render the report            
        ////    renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        ////    //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
        ////    if (format == null)
        ////    {
        ////        return File(renderedBytes, "image/jpeg");
        ////    }
        ////    else if (format == "PDF")
        ////    {
        ////        return File(renderedBytes, "pdf");
        ////    }
        ////    else
        ////    {
        ////        return File(renderedBytes, "image/jpeg");
        ////    }
        ////}

        ////public ActionResult DownloadReportInBetweenDate(string bookingarrivedrequesttodatetime, string bookingarrivedrequestfromdatetime, string bookingarrivedrequestdatetime1, string format)
        ////{

        ////    LocalReport localReport = new LocalReport();
        ////    localReport.ReportPath = Server.MapPath("~/Report/ReportBookingArrived.rdlc");
        ////    ReportDataSource reportDataSource = new ReportDataSource();
        ////    reportDataSource.Name = "DataSet1";
        ////    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        ////    ViewBag.CurrentFilter = bookingarrivedrequesttodatetime;
        ////    ViewBag.CurrentFilter = bookingarrivedrequestfromdatetime;
        ////    DateTime BookingArrivedEnquiredDateTime1 = DateTime.Parse(bookingarrivedrequesttodatetime, theCultureInfo);
        ////    DateTime BookingArrivedEnquiredDateTime2 = DateTime.Parse(bookingarrivedrequestfromdatetime, theCultureInfo);
        ////    if ((bookingarrivedrequestdatetime != null) || (bookingarrivedrequestdatetime1 != null))
        ////    {
        ////        //var BookingArrivedDatefilterList = from ba in db.BookingArriveds
        ////        //                                   join h in db.HotelDetails on ba.ConsultantId equals h.HotelId
        ////        //                                   where (ba.BookingArrivedEnquiredDateTime) == bookingarrivedrequestdatetime &&
        ////        //                                   (ba.HotelDetail.HotelName) == hotel
        ////        //                                   select ba;

        ////        var BookingArrivedDatefilterList = db.BookingArriveds.Where(x => DateTime.Parse(x.BookingArrivedEnquiredDateTime) >= DateTime.Parse(bookingarrivedrequestdatetime)
        ////                      && DateTime.Parse(x.BookingArrivedEnquiredDateTime) <= DateTime.Parse(bookingarrivedrequestdatetime1)).ToList();
        ////        reportDataSource.Value = BookingArrivedDatefilterList;
        ////    }
        ////    else

        ////        reportDataSource.Value = db.BookingArriveds;

        ////    localReport.DataSources.Add(reportDataSource);

        ////    string reportType = format;
        ////    // string Parameter = bookingarriveddate;
        ////    string mimeType;
        ////    string encoding;
        ////    string fileNameExtension;



        ////    string deviceInfo =

        ////       "<DeviceInfo>" +
        ////       "  <OutputFormat>" + format + "</OutputFormat>" +
        ////       "  <PageWidth>17in</PageWidth>" +
        ////       "  <PageHeight>10in</PageHeight>" +
        ////       "  <MarginTop>0.5in</MarginTop>" +
        ////       "  <MarginLeft>1.2in</MarginLeft>" +
        ////       "  <MarginRight>0.2in</MarginRight>" +
        ////       "  <MarginBottom>0.5in</MarginBottom>" +
        ////       "</DeviceInfo>";

        ////    Warning[] warnings;
        ////    string[] streams;
        ////    byte[] renderedBytes;

        ////    renderedBytes = localReport.Render(
        ////        reportType,
        ////        deviceInfo,
        ////        out mimeType,
        ////        out encoding,
        ////        out fileNameExtension,
        ////        out streams,
        ////        out warnings);


        ////    return File(renderedBytes, mimeType);
        ////}


    }
}