using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YCWeb.Data;

namespace YCWeb.Models
{
    public class VMUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Password { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string VerificationCode { get; set; }
        public string VerificationCodeTemp { get; set; }
        public bool resubmit { get; set; }
    }
}