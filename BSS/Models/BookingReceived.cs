using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace BSS.Models
{

    public class SearchParameterModel1
    {

        [Display(Name = "Search By BookingArrivedEnquiredToDateTime")]
        public string BookingArrivedEnquiredToDateTime
        {
            get;

            set;
        }
        [Display(Name = "Search By BookingArrivedEnquiredFromDateTime")]
        public string BookingArrivedEnquiredFromDateTime
        {
            get;

            set;
        }

        [Display(Name = "and BookingStatus")]
        public string BookingStatus
        {
            get;

            set;
        }

        [Display(Name = "and Agent")]
        public string Agent
        {
            get;

            set;
        }

        [Display(Name = "and Consultant")]
        public string Consultant
        {
            get;

            set;
        }

        [Display(Name = "and Hotel")]
        public string Hotel
        {
            get;

            set;
        }

        public string Format
        {
            get;

            set;
        }

    }

    public class BookingReceived
    {
        // [Key]
        private int m_BookingArrivedId;
        private string m_BookingArrivedEnquiredDateTime;
        private string m_BookingArrivedEnquiredTime;
        private string m_BookingArrivedEnquiredDetails;
        private string m_TravelDate;
        private int m_AgentId;
        private int m_ConsultantId;
        private string m_BookingType;
        private string m_QuotationSendDateTime;
        private string m_QuotationSendTime;
        private string m_TourPlanRefrence;
        private int m_HotelId;
        private string m_BookingStatus;
        private string m_ConfirmationDate;

        [Key]
        public int BookingArrivedId
        {
            get;
            set;
            //  get { return m_BookingArrivedId; }

            //  set { value = m_BookingArrivedId; }
        }


        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "Enq Arvd Date")]
        [Required]
        public DateTime? BookingArrivedEnquiredDateTime
        {
            get;
            set;

            //  get { return m_BookingArrivedEnquiredDateTime; }

            //  set { value = m_BookingArrivedEnquiredDateTime; }
        }

        [Display(Name = "Enq Rcvd Time")]
        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string BookingArrivedEnquiredTime
        {
            get;
            set;

            //get { return m_BookingArrivedEnquiredTime; }

            //set { value = m_BookingArrivedEnquiredTime; }
        }

        [Display(Name = "Enq Details")]
        [Required(ErrorMessage = "Please Enter Enquiry Details")]
        public string BookingArrivedEnquiredDetails
        {

            get;
            set;
            //get { return m_BookingArrivedEnquiredDetails; }

            //set { value = m_BookingArrivedEnquiredDetails; }
        }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Travel Date")]
        public DateTime? TravelDate
        {

            get;
            set;
            ////get { return m_TravelDate; }

            ////set { value = m_TravelDate; }
        }


        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Please Select Agent")]
        public int AgentId
        {

            get;
            set;
            //get { return m_AgentId; }

            //set { value = m_AgentId; }
        }

        [Display(Name = "CNSLT")]
        [Required(ErrorMessage = "Please Select Consultant")]
        public int ConsultantId
        {

            get;
            set;
            ////get { return m_ConsultantId; }

            ////set { value = m_ConsultantId; }
        }


        [Display(Name = "BKNG TYPE")]
        public string BookingType
        {

            get;
            set;
            //get { return m_BookingStatus; }

            //set { value = m_BookingStatus; }
        }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Quot Sent Date")]
        public DateTime? QuotationSendDateTime
        {

            get;
            set;
            //get { return m_QuotationSendDateTime; }

            //set { value = m_QuotationSendDateTime; }
        }

        [Display(Name = "Quot Sent Time")]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string QuotationSendTime
        {

            get;
            set;
            //get { return m_QuotationSendTime; }

            //set { value = m_QuotationSendTime; }
        }

        [Display(Name = "TP Ref")]
        public string TourPlanRefrence
        {

            get;
            set;
            //get { return m_TourPlanRefrence; }

            //set { value = m_TourPlanRefrence; }
        }

        [Display(Name = "Hotel")]
        [Required(ErrorMessage = "Please Select Hotel")]
        public int HotelId
        {

            get;
            set;
            //get { return m_HotelId; }

            //set { value = m_HotelId; }
        }

        [Display(Name = "BKNG ST")]
        public string BookingStatus
        {

            get;
            set;
            //get { return m_BookingStatus; }

            //set { value = m_BookingStatus; }
        }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .]([1-9]|0[1-9]|1[0-2])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$", ErrorMessage = "Invalid Date.")]
        [Display(Name = "Date CF")]
        public DateTime? ConfirmationDate
        {

            get;
            set;
            //get { return m_ConfirmationDate; }

            //set { value = m_ConfirmationDate; }
        }



        public BookingReceived() { }

        public BookingReceived(int bookingarrivedId, string bookingarrivedenquireddatetime, string bookingarrivedenquiredtime, string bookingarrivedenquireddetails,
            string traveldate, int agentId, int consultantId, string bookingtype, string quotationsenddatetime, string quotationsendtime, string tourplanrefrence,
            int hotelId, string bookingstatus, string confirmationdate)
        {
            m_BookingArrivedId = bookingarrivedId;
            m_BookingArrivedEnquiredDateTime = bookingarrivedenquireddatetime;
            m_BookingArrivedEnquiredTime = bookingarrivedenquiredtime;
            m_BookingArrivedEnquiredDetails = bookingarrivedenquireddetails;
            m_TravelDate = traveldate;
            m_AgentId = agentId;
            m_ConsultantId = consultantId;
            m_BookingType = bookingtype;
            m_QuotationSendDateTime = quotationsenddatetime;
            m_QuotationSendTime = quotationsendtime;
            m_TourPlanRefrence = tourplanrefrence;
            m_HotelId = hotelId;
            m_BookingStatus = bookingstatus;
            m_ConfirmationDate = confirmationdate;

        }

        public virtual HotelDetails HotelDetail { get; set; }

        public virtual AgentDetails AgentDetail { get; set; }

        public virtual ConsultantDetails ConsultantDetail { get; set; }

    }

}