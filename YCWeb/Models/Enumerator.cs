using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YCWeb.Models
{
    public static class Enumerator
    {
        public enum UserTypeId
        {
            User = 1,
            Admin = 2,
            Guest = 3
        }
    }
}