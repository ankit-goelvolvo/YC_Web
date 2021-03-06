﻿using System;
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
    public class UsersController : Controller
    {
        private YCEntities db = new YCEntities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.User1).Include(u => u.User2).Include(u => u.UserType);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName");
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserTypeId,FirstName,LastName,Email,Mobile,Phone,Address,DateOfBirth,IsActive,Gender,Photo,MemberId,CreatedBy,CreatedDate,UpdatedDate,UpdatedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", user.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", user.UpdatedBy);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeName", user.UserTypeId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", user.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", user.UpdatedBy);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeName", user.UserTypeId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserTypeId,FirstName,LastName,Email,Mobile,Phone,Address,DateOfBirth,IsActive,Gender,Photo,MemberId,CreatedBy,CreatedDate,UpdatedDate,UpdatedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "FirstName", user.CreatedBy);
            ViewBag.UpdatedBy = new SelectList(db.Users, "UserID", "FirstName", user.UpdatedBy);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeName", user.UserTypeId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
