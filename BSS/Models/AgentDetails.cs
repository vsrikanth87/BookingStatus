using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BSS.Models
{
    public class AgentDetails
    {
        [Key]
        public int AgentId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(50, ErrorMessage = "Name not be exceed 50 char")]
        public string AgentName { get; set; }

        [Display(Name = "Address")]
        public string AgentAddress { get; set; }

        [Display(Name = "ContactNo")]
        public string AgentContactNo { get; set; }

        public virtual ICollection<BookingArrived> BookingArriveds { get; set; }
    }
}