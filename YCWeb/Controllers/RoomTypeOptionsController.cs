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
    public class RoomTypeOptionsController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: RoomTypeOptions
        public ActionResult Index()
        {
            var roomTypeOptions = db.RoomTypeOptions.Include(r => r.OfficeFacility).Include(r => r.User).Include(r => r.RoomType).Include(r => r.User1);
            return View(roomTypeOptions.ToList());
        }

        // GET: RoomTypeOptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTypeOption roomTypeOption = db.RoomTypeOptions.Find(id);
            if (roomTypeOption == null)
            {
                return HttpNotFound();
            }
            return View(roomTypeOption);
        }

        // GET: RoomTypeOptions/Create
        public ActionResult Create()
        {
            ViewBag.OfficeFacilityID = new SelectList(db.OfficeFacilities, "OfficeFacilityID", "OfficeFacilityName");
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: RoomTypeOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomTypeOptionID,OfficeFacilityID,RoomTypeID,Price,IsRefundable,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] RoomTypeOption roomTypeOption)
        {
            if (ModelState.IsValid)
            {
                db.RoomTypeOptions.Add(roomTypeOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OfficeFacilityID = new SelectList(db.OfficeFacilities, "OfficeFacilityID", "OfficeFacilityName", roomTypeOption.OfficeFacilityID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomTypeOption.RoomTypeID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.UpdatedBy);
            return View(roomTypeOption);
        }

        // GET: RoomTypeOptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTypeOption roomTypeOption = db.RoomTypeOptions.Find(id);
            if (roomTypeOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfficeFacilityID = new SelectList(db.OfficeFacilities, "OfficeFacilityID", "OfficeFacilityName", roomTypeOption.OfficeFacilityID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomTypeOption.RoomTypeID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.UpdatedBy);
            return View(roomTypeOption);
        }

        // POST: RoomTypeOptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomTypeOptionID,OfficeFacilityID,RoomTypeID,Price,IsRefundable,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] RoomTypeOption roomTypeOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomTypeOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OfficeFacilityID = new SelectList(db.OfficeFacilities, "OfficeFacilityID", "OfficeFacilityName", roomTypeOption.OfficeFacilityID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.CreatedBy);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomTypeOption.RoomTypeID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.UpdatedBy);
            return View(roomTypeOption);
        }

        // GET: RoomTypeOptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTypeOption roomTypeOption = db.RoomTypeOptions.Find(id);
            if (roomTypeOption == null)
            {
                return HttpNotFound();
            }
            return View(roomTypeOption);
        }

        // POST: RoomTypeOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomTypeOption roomTypeOption = db.RoomTypeOptions.Find(id);
            db.RoomTypeOptions.Remove(roomTypeOption);
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
