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
            var roomTypeOptions = db.RoomTypeOptions.Include(r => r.OfficeFacility).Include(r => r.RoomType);
            List<Facilitiy> f = new List<Facilitiy>();
            var secondaryContents = (from c in roomTypeOptions
                                     group c by new
                                     {
                                         c.RoomTypeID,
                                         c.RoomType.RoomTypeName
                                     } into g
                                     select new
                                     {
                                         RoomtypeId = g.Key.RoomTypeID,
                                         RoomtypeName = g.Key.RoomTypeName,
                                         FacilityName = (from x in g.Select(n => n.OfficeFacility)
                                                         from fc in db.OfficeFacilities
                                                         where fc.OfficeFacilityID == x.OfficeFacilityID
                                                         select fc.OfficeFacilityName)
                                     }).ToList().Select(x => new
                                     {
                                         RoomtypeId = x.RoomtypeId,
                                         RoomtypeName = x.RoomtypeName.Trim(),
                                         FacilityName = x.FacilityName.Aggregate((names, next) => names.Trim() + ", " + next.Trim()).Trim()
                                     });
            foreach (var item in secondaryContents)
            {
                Facilitiy fac = new Facilitiy
                {
                    RoomtypeId = item.RoomtypeId,
                    RoomtypeName = item.RoomtypeName,
                    FacilityName = item.FacilityName
                };
                f.Add(fac);
            }
            return PartialView(f);
        }

        // GET: RoomTypeOptions/Details/5
        public ActionResult Details(int? id)
        {
            Facilitiy fac=null;
            var secondaryContents = (from c in db.RoomTypeOptions
                                     where c.RoomTypeID==id
                                     group c by new
                                     {
                                         c.RoomTypeID,
                                         c.RoomType.RoomTypeName
                                     } into g
                                     select new
                                     {
                                         RoomtypeId = g.Key.RoomTypeID,
                                         RoomtypeName = g.Key.RoomTypeName,
                                         FacilityName = (from x in g.Select(n => n.OfficeFacility)
                                                         from fc in db.OfficeFacilities
                                                         where fc.OfficeFacilityID == x.OfficeFacilityID
                                                         select fc.OfficeFacilityName),
                                         Price= g.Select(t=>t.Price),
                                         IsRefundable = g.Select(t => t.IsRefundable),
                                         CreatedBy = g.Select(t => t.User.FirstName),
                                         CreatedDate = g.Select(t => t.CreatedDate),
                                         UpdatedBy = g.Select(t => t.User1.FirstName),
                                         UpdatedDate = g.Select(t => t.UpdatedDate)

                                     }).ToList().Select(x => new
                                     {
                                         x.RoomtypeId,
                                         RoomtypeName = x.RoomtypeName.Trim(),
                                         FacilityName = x.FacilityName.Aggregate((names, next) => names.Trim() + ", " + next.Trim()).Trim(),
                                         x.Price,
                                         x.IsRefundable,
                                         x.CreatedBy,
                                         x.CreatedDate,
                                         x.UpdatedBy,
                                         x.UpdatedDate
                                     });
            foreach (var item in secondaryContents)
            {
                fac = new Facilitiy
                {
                    RoomtypeId = item.RoomtypeId,
                    RoomtypeName = item.RoomtypeName,
                    FacilityName = item.FacilityName,
                    Price=item.Price.FirstOrDefault(),
                    IsRefundable=item.IsRefundable.FirstOrDefault(),
                    CreatedBy=item.CreatedBy.FirstOrDefault(),
                    CreatedDate=item.CreatedDate.FirstOrDefault(),
                    UpdatedBy=item.UpdatedBy.FirstOrDefault(),
                    UpdatedDate=item.UpdatedDate.FirstOrDefault()
                };
            }
            if (fac == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Type Option not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(fac);
        }

        // GET: RoomTypeOptions/Create
        public PartialViewResult Create()
        {
            var vm = new RoomTypeOption();
            vm.Facilities = db.OfficeFacilities
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
                if (roomTypeOption.SelectedValues!=null)
                {
                    if (ModelState.IsValid)
                    {
                        var RoomType = db.RoomTypeOptions.Where(x => x.RoomTypeID == roomTypeOption.RoomTypeID).ToList();
                        if (RoomType.Count > 0)
                        {
                            var oids = roomTypeOption.SelectedValues.Select(id => int.Parse(id)).ToList();
                            var OfficeFacilities = RoomType.Where(t => oids.Contains(t.OfficeFacilityID));
                            if (OfficeFacilities.Count() > 0)
                            {
                                return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Office falility already present" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in roomTypeOption.SelectedValues)
                                {
                                    RoomTypeOption rto = new RoomTypeOption();
                                    rto.OfficeFacilityID = Int32.Parse(item);
                                    rto.RoomTypeID = roomTypeOption.RoomTypeID;
                                    rto.Price = roomTypeOption.Price;
                                    rto.IsRefundable = roomTypeOption.IsRefundable;
                                    rto.CreatedBy = (Session["User"] as SessionEntity).UserID;
                                    rto.CreatedDate = DateTime.Now;
                                    db.RoomTypeOptions.Add(rto);
                                    db.SaveChanges();
                                }
                                dbTran.Commit();
                            }
                            catch (Exception e)
                            {
                                dbTran.Rollback();
                                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office falilities Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
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
