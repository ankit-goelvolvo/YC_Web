using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YCWeb.Data;

namespace YCWeb.Controllers
{
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "RoomTypeOptionID");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomID,RoomName,Description,RoomTypeID,RoomTypeOptionID,MaxAdults,MaxChildren,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", room.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
            ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "RoomTypeOptionID", room.RoomTypeOptionID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", room.UpdatedBy);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", room.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
            ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "RoomTypeOptionID", room.RoomTypeOptionID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", room.UpdatedBy);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomID,RoomName,Description,RoomTypeID,RoomTypeOptionID,MaxAdults,MaxChildren,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", room.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", room.RoomTypeID);
            ViewBag.RoomTypeOptionID = new SelectList(db.RoomTypeOptions, "RoomTypeOptionID", "RoomTypeOptionID", room.RoomTypeOptionID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", room.UpdatedBy);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
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
