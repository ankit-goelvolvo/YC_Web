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
    using System.ComponentModel.DataAnnotations;

    public partial class RoomAmenity
    {
        public int RoomAmenitiesID { get; set; }
        [Required]
        public int RoomTypeID { get; set; }
        [Required]
        [Display(Name ="Room Amenity")]
        public string RoomAmenitiesName { get; set; }
        [Required]
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual User User { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual User User1 { get; set; }
    }
}
