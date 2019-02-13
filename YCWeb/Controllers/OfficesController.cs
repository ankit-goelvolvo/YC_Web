using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YCWeb.Data;
using YCWeb.Models;

namespace YCWeb.Controllers
{
    public class OfficesController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: Offices
        public ActionResult Index()
        {
            var offices = db.Offices.Include(o => o.Location).Include(o => o.User).Include(o => o.OfficeType).Include(o => o.User1);
            return PartialView(offices.ToList());
        }

        // GET: Offices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Office office = db.Offices.Find(id);
            if (office == null)
            {
                return HttpNotFound();
            }
            return View(office);
        }

        // GET: Offices/Create
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.OfficeTypeID = new SelectList(db.OfficeTypes, "OfficeTypeID", "OfficeTypeName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: Offices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateOffice(Office office)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.Offices.Where(x => x.OfficeName.Equals(office.OfficeName)).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Office already present" }, JsonRequestBehavior.AllowGet);
                    }
                    office.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    office.CreatedDate = DateTime.Now;
                    db.Offices.Add(office);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", office.LocationID);
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", office.CreatedBy);
                ViewBag.OfficeTypeID = new SelectList(db.OfficeTypes, "OfficeTypeID", "OfficeTypeName", office.OfficeTypeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", office.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }           

        // GET: Offices/Edit/5
        public ActionResult Edit(int? id)
        {
            Office office = db.Offices.Find(id);
            if (office == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", office.LocationID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", office.CreatedBy);
            ViewBag.OfficeTypeID = new SelectList(db.OfficeTypes, "OfficeTypeID", "OfficeTypeName", office.OfficeTypeID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", office.UpdatedBy);
            return PartialView(office);
        }

        // POST: Offices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateOffice(Office office)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(office).State = EntityState.Modified;
                    office.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    office.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", office.LocationID);
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", office.CreatedBy);
                ViewBag.OfficeTypeID = new SelectList(db.OfficeTypes, "OfficeTypeID", "OfficeTypeName", office.OfficeTypeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", office.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Offices/Delete/5
        public ActionResult Delete(int? id)
        {
            Office office = db.Offices.Find(id);
            if (office == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(office);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Office office = db.Offices.Find(id);
                db.Offices.Remove(office);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
