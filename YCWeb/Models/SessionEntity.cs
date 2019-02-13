using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YCWeb.Models
{
    public class SessionEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public int UserID { get; set; }
        public String Email { get; set; }
    }
}