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
    
    public partial class UpdateBilling_Result
    {
        public int BillingID { get; set; }
        public int HotelBookingID { get; set; }
        public System.DateTime BillingDate { get; set; }
        public decimal Price { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}