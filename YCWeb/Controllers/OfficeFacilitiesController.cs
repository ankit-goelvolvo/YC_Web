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
    public class OfficeFacilitiesController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: OfficeFacilities
        public ActionResult Index()
        {
            var officeFacilities = db.OfficeFacilities.Include(o => o.User).Include(o => o.Office).Include(o => o.User1);
            return PartialView(officeFacilities.ToList());
        }

        // GET: OfficeFacilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeFacility officeFacility = db.OfficeFacilities.Find(id);
            if (officeFacility == null)
            {
                return HttpNotFound();
            }
            return View(officeFacility);
        }

        // GET: OfficeFacilities/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: OfficeFacilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateOfficeFacility(OfficeFacility officeFacility)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.OfficeFacilities.Where(x => x.OfficeFacilityName.Equals(officeFacility.OfficeFacilityName)).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Office Facility already present" }, JsonRequestBehavior.AllowGet);
                    }
                    officeFacility.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    officeFacility.CreatedDate = DateTime.Now;
                    db.OfficeFacilities.Add(officeFacility);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Facility Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", officeFacility.CreatedBy);
                ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName", officeFacility.OfficeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", officeFacility.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: OfficeFacilities/Edit/5
        public ActionResult Edit(int? id)
        {
            OfficeFacility officeFacility = db.OfficeFacilities.Find(id);
            if (officeFacility == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office Facility not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", officeFacility.CreatedBy);
            ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName", officeFacility.OfficeID);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", officeFacility.UpdatedBy);
            return PartialView(officeFacility);
        }

        // POST: OfficeFacilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateOfficeFacility(OfficeFacility officeFacility)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(officeFacility).State = EntityState.Modified;
                    officeFacility.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    officeFacility.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Facility Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", officeFacility.CreatedBy);
                ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName", officeFacility.OfficeID);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", officeFacility.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: OfficeFacilities/Delete/5
        public ActionResult Delete(int? id)
        {
            OfficeFacility officeFacility = db.OfficeFacilities.Find(id);
            if (officeFacility == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office Faclility not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(officeFacility);
        }

        // POST: OfficeFacilities/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                OfficeFacility officeFacility = db.OfficeFacilities.Find(id);
                db.OfficeFacilities.Remove(officeFacility);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Faclility Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
