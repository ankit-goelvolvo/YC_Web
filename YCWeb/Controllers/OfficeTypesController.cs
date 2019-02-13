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
    public class OfficeTypesController : Controller
    {
        private YCEntities db = new YCEntities();
        

        // GET: OfficeTypes
        public ActionResult Index()
        {
            var officeTypes = db.OfficeTypes.Include(o => o.User).Include(o => o.User1);
            return PartialView(officeTypes.ToList());
        }

        // GET: OfficeTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeType officeType = db.OfficeTypes.Find(id);
            if (officeType == null)
            {
                return HttpNotFound();
            }
            return View(officeType);
        }

        // GET: OfficeTypes/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: OfficeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateOfficeType(OfficeType officeType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.OfficeTypes.Where(x => x.OfficeTypeName.Equals(officeType.OfficeTypeName)).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Office Type already present" }, JsonRequestBehavior.AllowGet);
                    }
                    officeType.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    officeType.CreatedDate = DateTime.Now;
                    db.OfficeTypes.Add(officeType);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Type Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: OfficeTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            OfficeType officeType = db.OfficeTypes.Find(id);
            if (officeType == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office Type not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(officeType);
        }

        // POST: OfficeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateOfficeTypes(OfficeType officeType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(officeType).State = EntityState.Modified;
                    officeType.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    officeType.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Type Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: OfficeTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            OfficeType officeType = db.OfficeTypes.Find(id);
            if (officeType == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office Type not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(officeType);
        }

        // POST: OfficeTypes/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                OfficeType officeType = db.OfficeTypes.Find(id);
                db.OfficeTypes.Remove(officeType);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office Type Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
