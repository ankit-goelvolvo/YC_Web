using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCWeb.Data;

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
        public int CreatedBy { get; set; }
        [Display(Name = "Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [Display(Name = "Updated Date")]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> Facilities { set; get; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RoomTypeOptionsFacility> RoomTypeOptionsFacilities { get; set; }
        public virtual ICollection<FacilityOption> RoomTypeFacilities { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual User User1 { get; set; }
        public string[] SelectedValues { get; set; }

        public string SelectedFacility { get; set; }

        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public int OfficeFacilityID { get; set; }
    }
}