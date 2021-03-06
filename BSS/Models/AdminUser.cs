﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BSS.Models
{
    public class AdminUser
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "UserName")]
        [Required(ErrorMessage = "Please Enter UserName")]
        [StringLength(15, ErrorMessage = "Name not be exceed 15 char")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}