//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YCWeb.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Verification
    {
        public int VerificationId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string VerificationCode { get; set; }
        public bool IsMobileVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public Nullable<System.DateTime> MobileVerifyDate { get; set; }
        public Nullable<System.DateTime> EmailVerifyDate { get; set; }
    
        public virtual User User { get; set; }
    }
}
