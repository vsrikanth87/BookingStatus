using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BSS.Models
{
    public class HotelDetails
    {
        [Key]
        public int HotelId { get; set; }

        [Display(Name = "Name")]
        //[Required(ErrorMessage = "Please Enter Name")]
        [StringLength(50, ErrorMessage = "Name not be exceed 50 char")]
        public string HotelName { get; set; }

        [Display(Name = "Address")]
        public string HotelAddress { get; set; }

        [Display(Name = "ContactNo")]
        public string HotelContactNo { get; set; }

        public virtual ICollection<BookingArrived> BookingArriveds { get; set; }
    }
}