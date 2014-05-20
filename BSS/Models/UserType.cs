using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BSS.Models
{
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        [Display(Name = "UserType")]
        [Required(ErrorMessage = "Please Enter UserType")]
        public string UserTypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}