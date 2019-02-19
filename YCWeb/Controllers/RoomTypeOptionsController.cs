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
    public class RoomTypeOptionsController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: RoomTypeOptions
        public ActionResult Index()
        {
            var roomTypeOptions = db.RoomTypeOptions.Include(r => r.OfficeFacility).Include(r => r.User).Include(r => r.RoomType).Include(r => r.User1);
            return PartialView(roomTypeOptions.ToList());
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
        public PartialViewResult Create()
        {
            var vm = new RoomTypeOption();
            vm.Facilities= db.OfficeFacilities
            .Select(x => new SelectListItem
            {
                Value = x.OfficeFacilityID.ToString(),
                Text = x.OfficeFacilityName,
            })
            .ToList();
            //ViewBag.OfficeFacilityID = new SelectList(db.OfficeFacilities, "OfficeFacilityID", "OfficeFacilityName");
            //ViewBag.OfficeFacility = db.OfficeFacilities
            //.Select(x => new SelectListItem
            //{
            //    Value = x.OfficeFacilityID.ToString(),
            //    Text = x.OfficeFacilityName,
            //})
            //.ToList();
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView(vm);
        }

        // POST: RoomTypeOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateRoomTypeOption(RoomTypeOption roomTypeOption)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.RoomTypeOptions.Where(x => x.RoomTypeID== roomTypeOption.RoomTypeID && x.OfficeFacilityID== roomTypeOption.OfficeFacilityID).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Room Type Option already present" }, JsonRequestBehavior.AllowGet);
                    }
                    roomTypeOption.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    roomTypeOption.CreatedDate = DateTime.Now;
                    db.RoomTypeOptions.Add(roomTypeOption);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Type Option Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.OfficeFacilityID = new SelectList(db.OfficeFacilities, "OfficeFacilityID", "OfficeFacilityName", roomTypeOption.OfficeFacilityID);
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.CreatedBy);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomTypeOption.RoomTypeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
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
