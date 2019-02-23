using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YCWeb.Models
{
    public class Facilitiy
    {
        public int RoomTypeOptionID { get; set; }
        public int RoomtypeId { get; set; }
        [Display(Name = "Room Type")]
        public string RoomtypeName { get; set; }
        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Is Refundable")]
        public Nullable<bool> IsRefundable { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [Display(Name = "Updated Date")]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}