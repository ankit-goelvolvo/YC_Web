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
    public class PoliciesController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: Policies
        public ActionResult Index()
        {
            var policies = db.Policies.Include(p => p.Office).Include(p => p.User).Include(p => p.User1);
            return PartialView(policies.ToList());
        }

        // GET: Policies/Details/5
        public ActionResult Details(int? id)
        {
            Policy policy = db.Policies.Find(id);
            if (policy == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Policy not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(policy);
        }

        // GET: Policies/Create
        public ActionResult Create()
        {
            ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName");
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: Policies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreatePolicy(Policy policy)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.Policies.Where(x => x.PolicyName.Equals(policy.PolicyName)).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Policy already present" }, JsonRequestBehavior.AllowGet);
                    }
                    policy.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    policy.CreatedDate = DateTime.Now;
                    db.Policies.Add(policy);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Policy Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName", policy.OfficeID);
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", policy.CreatedBy);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", policy.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }
        

        // GET: Policies/Edit/5
        public ActionResult Edit(int? id)
        {
            Policy policy = db.Policies.Find(id);
            if (policy == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Policy not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName", policy.OfficeID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", policy.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", policy.UpdatedBy);
            return PartialView(policy);
        }

        // POST: Policies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdatePolicy(Policy policy)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(policy).State = EntityState.Modified;
                    policy.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    policy.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Policy Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.OfficeID = new SelectList(db.Offices, "OfficeID", "OfficeName", policy.OfficeID);
                ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", policy.CreatedBy);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", policy.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Policies/Delete/5
        public ActionResult Delete(int? id)
        {
            Policy policy = db.Policies.Find(id);
            if (policy == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Policy not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(policy);
        }

        // POST: Policies/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Policy policy = db.Policies.Find(id);
                db.Policies.Remove(policy);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Policy Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
