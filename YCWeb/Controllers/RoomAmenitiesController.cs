using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YCWeb.Data;
using YCWeb.Filter;
using YCWeb.Models;

namespace YCWeb.Controllers
{
    [CustomActionFilter]
    public class RoomAmenitiesController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: RoomAmenities
        public ActionResult Index()
        {
            var roomAmenities = db.RoomAmenities.Include(r => r.User).Include(r => r.RoomType).Include(r => r.User1).OrderBy(r=>r.RoomType.RoomTypeName);
            return PartialView(roomAmenities.ToList());
        }

        // GET: RoomAmenities/Details/5
        public ActionResult Details(int? id)
        {
            RoomAmenity roomAmenity = db.RoomAmenities.Find(id);
            if (roomAmenity == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Amenities not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(roomAmenity);
        }

        // GET: RoomAmenities/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: RoomAmenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateRoomAmenities(RoomAmenity roomAmenity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.RoomAmenities.Where(x => x.RoomAmenitiesName.ToUpper().Equals(roomAmenity.RoomAmenitiesName.ToUpper())).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Room Amenity already present" }, JsonRequestBehavior.AllowGet);
                    }
                    roomAmenity.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    roomAmenity.CreatedDate = DateTime.Now;
                    db.RoomAmenities.Add(roomAmenity);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Amenity Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomAmenity.CreatedBy);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomAmenity.RoomTypeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomAmenity.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: RoomAmenities/Edit/5
        public ActionResult Edit(int? id)
        {
            RoomAmenity roomAmenity = db.RoomAmenities.Find(id);
            if (roomAmenity == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Amenity not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomAmenity.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomAmenity.RoomTypeID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomAmenity.UpdatedBy);
            return PartialView(roomAmenity);
        }

        // POST: RoomAmenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateRoomAmenities(RoomAmenity roomAmenity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(roomAmenity).State = EntityState.Modified;
                    roomAmenity.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    roomAmenity.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Amenity Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomAmenity.CreatedBy);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomAmenity.RoomTypeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomAmenity.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: RoomAmenities/Delete/5
        public ActionResult Delete(int? id)
        {
            RoomAmenity roomAmenity = db.RoomAmenities.Find(id);
            if (roomAmenity == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Amenity not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(roomAmenity);
        }

        // POST: RoomAmenities/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                RoomAmenity roomAmenity = db.RoomAmenities.Find(id);
                db.RoomAmenities.Remove(roomAmenity);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Amenity Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
