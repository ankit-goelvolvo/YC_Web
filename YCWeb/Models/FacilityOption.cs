using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YCWeb.Models
{
    public class FacilityOption
    {
        public int RoomTypeOptionsFacilityID { get; set; }
        public int RoomTypeOptionID{ get; set; }

        public string FacilityName { get; set; }
    }
}