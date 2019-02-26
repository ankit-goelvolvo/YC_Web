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
    public class RoomsController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.User).Include(r => r.RoomType).Include(r => r.RoomTypeOption).Include(r => r.User1);
            return PartialView(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "Description");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateRooms(Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.Rooms.Where(x => x.RoomName.ToUpper().Equals(room.RoomName.ToUpper())).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Room already present" }, JsonRequestBehavior.AllowGet);
                    }
                    room.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    room.CreatedDate = DateTime.Now;
                    db.Rooms.Add(room);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
                ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "Description");
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }
                
        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", room.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
            ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "Description", room.RoomTypeOptionID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", room.UpdatedBy);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateRooms(Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(room).State = EntityState.Modified;
                    room.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    room.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", room.CreatedBy);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
                ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "Description", room.RoomTypeOptionID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", room.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(room);
        }

        // POST: Rooms/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Room room = db.Rooms.Find(id);
                db.Rooms.Remove(room);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
