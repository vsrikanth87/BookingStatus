using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace BSS.Models
{
    public class ConsultantDetails
    {
        [Key]
        public int ConsultantId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(50, ErrorMessage = "Name not be exceed 50 char")]
        public string ConsultantName { get; set; }

        [Display(Name = "Address")]
        public string ConsultantAddress { get; set; }

        [Display(Name = "ContactNo")]
        public string ConsultantContactNo { get; set; }

        public virtual ICollection<BookingArrived> BookingArriveds { get; set; }
    }
}