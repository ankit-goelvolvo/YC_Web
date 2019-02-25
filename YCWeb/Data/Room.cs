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
    
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            this.HotelBookings = new HashSet<HotelBooking>();
        }
    
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public int RoomTypeID { get; set; }
        public int RoomTypeOptionID { get; set; }
        public Nullable<int> MaxAdults { get; set; }
        public Nullable<int> MaxChildren { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelBooking> HotelBookings { get; set; }
        public virtual User User { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual User User1 { get; set; }
        public virtual RoomTypeOption RoomTypeOption { get; set; }
    }
}
