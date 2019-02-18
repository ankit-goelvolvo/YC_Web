using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using YCWeb.Filter;
using YCWeb.Data;
using YCWeb.Models;
using System;

namespace YCWeb.Controllers
{
    [CustomActionFilter]
    public class RoomTypesController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: RoomTypes
        public ActionResult Index()
        {
            var roomTypes = db.RoomTypes.Include(r => r.User).Include(r => r.User1);
            return PartialView(roomTypes.ToList());
        }

        // GET: RoomTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomType roomType = db.RoomTypes.Find(id);
            if (roomType == null)
            {
                return HttpNotFound();
            }
            return View(roomType);
        }

        // GET: RoomTypes/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView();
        }

        // POST: RoomTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateRoomType(RoomType roomType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int totalExistRows = db.RoomTypes.Where(x => x.RoomTypeName.Equals(roomType.RoomTypeName)).Count();
                    if (totalExistRows > 0)
                    {
                        return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Room Type already present" }, JsonRequestBehavior.AllowGet);
                    }
                    roomType.CreatedBy = (Session["User"] as SessionEntity).UserID;
                    roomType.CreatedDate = DateTime.Now;
                    db.RoomTypes.Add(roomType);
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Type Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: RoomTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            RoomType roomType = db.RoomTypes.Find(id);
            if (roomType == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Type not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", roomType.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomType.UpdatedBy);
            return PartialView(roomType);
        }

        // POST: RoomTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateRoomTypes(RoomType roomType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(roomType).State = EntityState.Modified;
                    roomType.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                    roomType.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Type Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        // GET: RoomTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            RoomType roomType = db.RoomTypes.Find(id);
            if (roomType == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Type not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(roomType);
        }

        // POST: RoomTypes/Delete/5
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                RoomType roomType = db.RoomTypes.Find(id);
                db.RoomTypes.Remove(roomType);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Room Type Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
