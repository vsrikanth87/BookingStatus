using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BSS.Models;

namespace BSS
{
    public class BSSDbContext : DbContext
    {
        public DbSet<BookingArrived> BookingArriveds
        {
            get;
            set;
        }

        public DbSet<BookingReceived> BookingReceiveds
        {
            get;
            set;
        }

        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<UserType> UserTypes
        {
            get;
            set;
        }


        public DbSet<AgentDetails> AgentDetails 
        { 
            get; set; 
        }

        public DbSet<ConsultantDetails> ConsultantDetails 
        { 
            get; set; 
        }

        public DbSet<HotelDetails> HotelDetails 
        { 
            get; set; 
        }

        public DbSet<Login> Logins 
        { 
            get; set; 
        }

        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}