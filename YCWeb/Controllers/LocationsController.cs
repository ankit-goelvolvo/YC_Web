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
    public class LocationsController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: Locations
        public ActionResult Index()
        {
            var locations = db.Locations.Include(l => l.User).Include(l => l.User1);
            return PartialView(locations.ToList());
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Location not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        public JsonResult CreateLocation(Location location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.Locations.Where(x => x.LocationName.Equals(location.LocationName)).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Location already present" }, JsonRequestBehavior.AllowGet);
                    }
                    location.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    location.CreatedDate = DateTime.Now;
                    db.Locations.Add(location);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Location Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Location not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        public JsonResult UpdateLocation(Location location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(location).State = EntityState.Modified;
                    location.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    location.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Location Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Location not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(location);
        }

        // POST: Locations/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Location location = db.Locations.Find(id);
                db.Locations.Remove(location);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Location Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
