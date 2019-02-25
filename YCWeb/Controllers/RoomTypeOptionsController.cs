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
            var roomTypeOptions = (from rto in db.RoomTypeOptions
                                   join rt in db.RoomTypes on rto.RoomTypeID equals rt.RoomTypeID
                                   join rtof in db.RoomTypeOptionsFacilities on rto.RoomTypeOptionID equals rtof.RoomTypeOptionID
                                   join fac in db.OfficeFacilities on rtof.OfficeFacilityID equals fac.OfficeFacilityID
                                   select new { rto, rt, rtof, fac });
            List <Facilitiy> f = new List<Facilitiy>();
            var secondaryContents = (from c in roomTypeOptions
                                     group c by new
                                     {
                                         c.rt.RoomTypeID,
                                         c.rt.RoomTypeName,
                                         c.rto.RoomTypeOptionID,
                                         c.rto.Description
                                     } into g
                                     select new
                                     {
                                         RoomTypeOptionID=g.Key.RoomTypeOptionID,
                                         RoomtypeId = g.Key.RoomTypeID,
                                         RoomtypeName = g.Key.RoomTypeName,
                                         g.Key.Description,
                                         FacilityName = (from x in g.Select(n => n.fac)
                                                         from fc in db.OfficeFacilities
                                                         where fc.OfficeFacilityID == x.OfficeFacilityID
                                                         select fc.OfficeFacilityName)
                                     }).ToList().Select(x => new
                                     {
                                         x.RoomTypeOptionID,
                                         x.RoomtypeId,
                                         RoomtypeName = x.RoomtypeName.Trim(),
                                         FacilityName = x.FacilityName.Aggregate((names, next) => names.Trim() + ", " + next.Trim()).Trim(),
                                         x.Description
                                     });
            foreach (var item in secondaryContents)
            {
                Facilitiy fac = new Facilitiy
                {
                    RoomtypeId = item.RoomtypeId,
                    RoomtypeName = item.RoomtypeName,
                    FacilityName = item.FacilityName,
                    RoomTypeOptionID=item.RoomTypeOptionID,
                    Description=item.Description
                };
                f.Add(fac);
            }
            return PartialView(f);
        }

        // GET: RoomTypeOptions/Details/5
        public ActionResult Details(int? id)
        {
            var roomTypeOptions = (from rto in db.RoomTypeOptions
                                   join rt in db.RoomTypes on rto.RoomTypeID equals rt.RoomTypeID
                                   join rtof in db.RoomTypeOptionsFacilities on rto.RoomTypeOptionID equals rtof.RoomTypeOptionID
                                   join facl in db.OfficeFacilities on rtof.OfficeFacilityID equals facl.OfficeFacilityID
                                   select new { rto, rt, rtof, facl });
            Facilitiy fac = null;
            var secondaryContents = (from c in roomTypeOptions
                                     where c.rto.RoomTypeOptionID == id
                                     group c by new
                                     {
                                         c.rt.RoomTypeID,
                                         c.rt.RoomTypeName,
                                         c.rto.RoomTypeOptionID,
                                         c.rto.Description
                                     } into g
                                     select new
                                     {
                                         RoomtypeId = g.Key.RoomTypeID,
                                         RoomtypeName = g.Key.RoomTypeName,
                                         FacilityName = (from x in g.Select(n => n.facl)
                                                         from fc in db.OfficeFacilities
                                                         where fc.OfficeFacilityID == x.OfficeFacilityID
                                                         select fc.OfficeFacilityName),
                                         Price = g.Select(t => t.rto.Price),
                                         IsRefundable = g.Select(t => t.rto.IsRefundable),
                                         CreatedBy = g.Select(t => t.rto.User.FirstName),
                                         CreatedDate = g.Select(t => t.rto.CreatedDate),
                                         UpdatedBy = g.Select(t => t.rto.User1.FirstName),
                                         UpdatedDate = g.Select(t => t.rto.UpdatedDate),
                                         g.Key.Description,

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
                                         x.UpdatedDate,
                                         x.Description
                                     });
            foreach (var item in secondaryContents)
            {
                fac = new Facilitiy
                {
                    RoomtypeId = item.RoomtypeId,
                    RoomtypeName = item.RoomtypeName,
                    FacilityName = item.FacilityName,
                    Price = item.Price.FirstOrDefault(),
                    IsRefundable = item.IsRefundable.FirstOrDefault(),
                    CreateBy = item.CreatedBy.FirstOrDefault(),
                    CreatedDate = item.CreatedDate.FirstOrDefault(),
                    UpdatedBy = item.UpdatedBy.FirstOrDefault(),
                    UpdatedDate = item.UpdatedDate.FirstOrDefault(),
                    Description=item.Description
                };
            }
            if (fac == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Type Option not found" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(fac);
        }

        //// GET: RoomTypeOptions/Create
        public PartialViewResult Create()
        {
            var vm = new Facilitiy();
            vm.Facilities = db.OfficeFacilities
            .Select(x => new SelectListItem
            {
                Value = x.OfficeFacilityID.ToString(),
                Text = x.OfficeFacilityName,
            })
            .ToList();
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            return PartialView(vm);
        }

        //// POST: RoomTypeOptions/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult CreateRoomTypeOption(Facilitiy roomTypeOption)
        {
            try
            {
                if (roomTypeOption.SelectedValues != null)
                {
                    if (ModelState.IsValid)
                    {
                        var roomTypeOptions = (from rto in db.RoomTypeOptions
                                               join rt in db.RoomTypes on rto.RoomTypeID equals rt.RoomTypeID
                                               join rtof in db.RoomTypeOptionsFacilities on rto.RoomTypeOptionID equals rtof.RoomTypeOptionID
                                               join facl in db.OfficeFacilities on rtof.OfficeFacilityID equals facl.OfficeFacilityID
                                               where rt.RoomTypeID == roomTypeOption.RoomtypeId
                                               select new { rto, rt, rtof, facl });

                        if (roomTypeOptions.Count() > 0)
                        {
                            var oids = roomTypeOption.SelectedValues.Select(id => int.Parse(id)).ToList();
                            var OfficeFacilities = roomTypeOptions.Where(t => oids.Contains(t.facl.OfficeFacilityID));
                            if (OfficeFacilities.Count() == roomTypeOptions.Select(c => c.facl.OfficeFacilityID).Count())
                            {
                                return Json(new { StatusCode = HttpStatusCode.Found, StatusMessage = "Office falilities already present" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                        {
                            try
                            {
                                RoomTypeOption rto = new RoomTypeOption();
                                //rto.OfficeFacilityID = Int32.Parse(item);
                                rto.RoomTypeID = roomTypeOption.RoomtypeId;
                                rto.Price = roomTypeOption.Price;
                                rto.IsRefundable = roomTypeOption.IsRefundable;
                                rto.Description = roomTypeOption.Description;
                                rto.CreatedBy = (Session["User"] as SessionEntity).UserID;
                                rto.CreatedDate = DateTime.Now;
                                db.RoomTypeOptions.Add(rto);
                                db.SaveChanges();


                                foreach (var item in roomTypeOption.SelectedValues)
                                {
                                    RoomTypeOptionsFacility rtof = new RoomTypeOptionsFacility();
                                    rtof.RoomTypeOptionID = rto.RoomTypeOptionID;
                                    rtof.OfficeFacilityID = Int32.Parse(item);
                                    rtof.CreatedBy = (Session["User"] as SessionEntity).UserID;
                                    rtof.CreatedDate = DateTime.Now;
                                    db.RoomTypeOptionsFacilities.Add(rtof);
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
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", roomTypeOption.RoomtypeId);
                ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", roomTypeOption.UpdatedBy);
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        //// GET: RoomTypeOptions/Edit/5
        public ActionResult Edit(int? id)
        {
            var roomTypeOptions = (from rto in db.RoomTypeOptions
                                   join rt in db.RoomTypes on rto.RoomTypeID equals rt.RoomTypeID
                                   join rtof in db.RoomTypeOptionsFacilities on rto.RoomTypeOptionID equals rtof.RoomTypeOptionID
                                   join facl in db.OfficeFacilities on rtof.OfficeFacilityID equals facl.OfficeFacilityID
                                   select new { rto, rt, rtof, facl });

            var secondaryContents = (from c in roomTypeOptions
                                     where c.rto.RoomTypeOptionID == id
                                     group c by new
                                     {
                                         c.rt.RoomTypeID,
                                         c.rt.RoomTypeName,
                                         c.rto.RoomTypeOptionID,
                                         c.rto.Description
                                     } into g
                                     select new
                                     {
                                         RoomtypeId = g.Key.RoomTypeID,
                                         RoomtypeName = g.Key.RoomTypeName,
                                         g.Key.Description,
                                         FacilityName = (from x in g.Select(n => n.facl)
                                                         from fc in db.OfficeFacilities
                                                         where fc.OfficeFacilityID == x.OfficeFacilityID
                                                         select fc.OfficeFacilityID),
                                         Price = g.Select(t => t.rto.Price),
                                         IsRefundable = g.Select(t => t.rto.IsRefundable),
                                         CreateBy = g.Select(t => t.rto.User.FirstName),
                                         CreatedBy = g.Select(t => t.rto.User.UserID),
                                         CreatedDate = g.Select(t => t.rto.CreatedDate),
                                         UpdateBy = g.Select(t => t.rto.User1.FirstName),
                                         g.Key.RoomTypeOptionID
                                     }).ToList().Select(x => new
                                     {
                                         x.RoomtypeId,
                                         RoomtypeName = x.RoomtypeName.Trim(),
                                         FacilityName = string.Join(",", x.FacilityName.Select(n => n.ToString()).ToArray()),
                                         x.Price,
                                         x.IsRefundable,
                                         x.CreateBy,
                                         x.CreatedBy,
                                         x.CreatedDate,
                                         x.UpdateBy,
                                         x.Description,
                                         x.RoomTypeOptionID,
                                     }).FirstOrDefault();
            var vm = new Facilitiy();
            vm.Facilities = db.OfficeFacilities
            .Select(x => new SelectListItem
            {
                Value = x.OfficeFacilityID.ToString(),
                Text = x.OfficeFacilityName,
            })
            .ToList();
            vm.SelectedValues = secondaryContents.FacilityName.Split(new string[] { "," }, StringSplitOptions.None);
            vm.RoomtypeId = secondaryContents.RoomtypeId;
            vm.Price = secondaryContents.Price.FirstOrDefault();
            vm.IsRefundable = secondaryContents.IsRefundable.FirstOrDefault();
            vm.CreateBy = secondaryContents.CreateBy.FirstOrDefault();
            vm.CreatedDate = secondaryContents.CreatedDate.FirstOrDefault();
            vm.UpdateBy = secondaryContents.UpdateBy.FirstOrDefault();
            vm.CreatedBy = secondaryContents.CreatedBy.FirstOrDefault();
            vm.Description = secondaryContents.Description;
            vm.RoomTypeOptionID = secondaryContents.RoomTypeOptionID;
            if (vm == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Office falility not found" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "RoomTypeName", vm.RoomtypeId);
            return PartialView(vm);
        }

        //// POST: RoomTypeOptions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public JsonResult UpdateRoomTypeOptions(Facilitiy roomTypeOption)
        {
            try
            {
                if (roomTypeOption.SelectedValues != null)
                {
                    if (ModelState.IsValid)
                    {
                        var roomTypeOptions = (from rto in db.RoomTypeOptions
                                               join rtof in db.RoomTypeOptionsFacilities on rto.RoomTypeOptionID equals rtof.RoomTypeOptionID
                                               where rto.RoomTypeOptionID == roomTypeOption.RoomTypeOptionID
                                               select new { rto, rtof });
                        
                        if (roomTypeOptions.Count() > 0)
                        {
                            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                            {
                                try
                                {
                                    RoomTypeOption rto = db.RoomTypeOptions.Where(x => x.RoomTypeOptionID == roomTypeOption.RoomTypeOptionID).FirstOrDefault();
                                    rto.RoomTypeID = roomTypeOption.RoomtypeId;
                                    rto.Price = roomTypeOption.Price;
                                    rto.IsRefundable = roomTypeOption.IsRefundable;
                                    rto.Description = roomTypeOption.Description;
                                    rto.UpdatedBy= (Session["User"] as SessionEntity).UserID;
                                    rto.UpdatedDate = DateTime.Now;
                                    db.Entry(rto).State = EntityState.Modified;
                                    db.SaveChanges();

                                    var rtoDel = db.RoomTypeOptionsFacilities.Where(x => x.RoomTypeOptionID == roomTypeOption.RoomTypeOptionID);
                                    //Removing all records from RoomTypeOptions with Room type
                                    if (rtoDel.Count()>0)
                                    {
                                        db.RoomTypeOptionsFacilities.RemoveRange(rtoDel);
                                        db.SaveChanges(); 
                                    }

                                    //Readding the all
                                    foreach (var item in roomTypeOption.SelectedValues)
                                    {
                                        RoomTypeOptionsFacility rtof = new RoomTypeOptionsFacility();
                                        rtof.RoomTypeOptionID = roomTypeOption.RoomTypeOptionID;
                                        rtof.OfficeFacilityID= Int32.Parse(item);
                                        rtof.CreatedBy = roomTypeOption.CreatedBy;
                                        rtof.CreatedDate = roomTypeOption.CreatedDate;
                                        rtof.UpdatedBy = (Session["User"] as SessionEntity).UserID;
                                        rtof.UpdatedDate = DateTime.Now;
                                        db.RoomTypeOptionsFacilities.Add(rtof);
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
                        }
                        return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office falility Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
        }

        //// GET: RoomTypeOptions/Delete/5
        public ActionResult Delete(int id)
        {
            Facilitiy f = new Facilitiy();
            var roomTypeOptions = (from rto in db.RoomTypeOptions
                                   join rt in db.RoomTypes on rto.RoomTypeID equals rt.RoomTypeID
                                   join rtof in db.RoomTypeOptionsFacilities on rto.RoomTypeOptionID equals rtof.RoomTypeOptionID
                                   join facl in db.OfficeFacilities on rtof.OfficeFacilityID equals facl.OfficeFacilityID
                                   where rto.RoomTypeOptionID == id
                                   select new { rto, rt, rtof, facl });
            
            if (roomTypeOptions.Count() < 0)
            {
                return Json(new { StatusCode = HttpStatusCode.NoContent, StatusMessage = "Room Type Option not found" }, JsonRequestBehavior.AllowGet);
            }
            var rtoption = roomTypeOptions.Where(y => y.rto.RoomTypeOptionID == id).FirstOrDefault();
            f.RoomtypeName = rtoption.rt.RoomTypeName;
            f.Price = rtoption.rto.Price;
            f.RoomTypeOptionID = id;
            f.Description = rtoption.rto.Description;

            f.RoomTypeFacilities = new List<FacilityOption>();
            var facilities = roomTypeOptions.Where(x => x.rto.RoomTypeOptionID == id);
            foreach (var item in facilities)
            {
                FacilityOption rtof = new FacilityOption
                {
                    RoomTypeOptionsFacilityID = item.rtof.RoomTypeOptionsFacilityID,
                    RoomTypeOptionID = item.rto.RoomTypeOptionID,
                    FacilityName = item.facl.OfficeFacilityName
                };
                f.RoomTypeFacilities.Add(rtof);
            }
            return PartialView(f);
        }

        // POST: RoomTypeOptions/Delete/5

        public JsonResult DeleteConfirmed(int rtoId, int rtofId)
        {
            try
            {
                RoomTypeOptionsFacility rtof = db.RoomTypeOptionsFacilities.Find(rtofId);
                db.RoomTypeOptionsFacilities.Remove(rtof);
                db.SaveChanges();
                return Json(new { StatusCode = HttpStatusCode.Created, StatusMessage = "Office facility Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
