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
using System.Collections.Specialized;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace YCWeb.Controllers
{
    public class LoginController : Controller
    {
        private YCEntities db = new YCEntities();
        private static Random random = new Random();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Login(Member member)
        {
            if (ModelState.IsValid)
            {
                member.username = member.username.Trim();
                member.password = member.password.Trim();
                var user = (from u in db.Users
                            join ul in db.UsersLogins on u.UserID equals ul.UserID
                            where ((u.Email.Equals(member.username)) || (u.Mobile == member.username)) && ul.Password.Equals(member.password, StringComparison.CurrentCulture)
                            select new SessionEntity
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                LastLoginDateTime = ul.LastLoginDateTime,
                                UserID = u.UserID,
                                Email = u.Email
                            }).FirstOrDefault();
                if (user != null)
                {
                    Session["User"] = user;
                    UsersLogin userlogin = db.UsersLogins.Where(x => x.UserID == user.UserID).FirstOrDefault();
                    userlogin.LastLoginDateTime = DateTime.Now;
                    userlogin.UpdatedDate = DateTime.Now;
                    userlogin.UpdatedBy = user.UserID;
                    userlogin.CreatedDate = DateTime.Now;
                    userlogin.CreatedBy = user.UserID;
                    db.Entry(userlogin).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { StatusCode = HttpStatusCode.Created }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { StatusCode = HttpStatusCode.MethodNotAllowed, StatusMessage = "Please enter required fields" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = HttpStatusCode.NotFound, StatusMessage = "User not found,Please register!!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignUp()
        {
            VMUser viewmodel = new VMUser();
           
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeName");
            ViewBag.Gender = new SelectList(FetchGender(), "Value", "Text");            
            return View(viewmodel);
        }

        internal List<SelectListItem> FetchGender()
        {
            List<SelectListItem> listGender = new List<SelectListItem>();
            listGender.Add(new SelectListItem { Value = "Male", Text = "Male" });
            listGender.Add(new SelectListItem { Value = "Female", Text = "Female" });
            return listGender;
        }

        [HttpPost]
        public ActionResult SignUp(VMUser viewmodel)
        {
            if (viewmodel.VerificationCode==null)
            {
                string sendSms = SendSMS(viewmodel);
                JObject results = JObject.Parse(sendSms);

                if (results["errors"] != null)
                {
                    foreach (var result in results["errors"])
                    {
                        viewmodel.StatusCode = 1;
                        viewmodel.StatusMessage = (string)result["message"];
                    }
                }
                else if (results["status"].ToString() == "success")
                {
                    viewmodel.StatusCode = 2;
                    viewmodel.StatusMessage = "Verification code sent. Please verify.";
                } 
            }
            //viewmodel.VerificationCodeTemp = "9K3NPO";
            //viewmodel.StatusCode = 2;
            //viewmodel.StatusMessage = "Verification code sent. Please verify.";
            if (viewmodel.StatusCode == 1)
            {
                return View(viewmodel);
            }

            if (viewmodel.VerificationCodeTemp != null)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        int totalExistRows = db.Users.Where(x => x.Email.ToUpper().Equals(viewmodel.Email.ToUpper())).Count();
                        if (totalExistRows > 0)
                        {
                            viewmodel.StatusCode = 3;
                            viewmodel.StatusMessage = "User already present";
                            return View(viewmodel);
                        }
                        User user = new User()
                        {
                            FirstName = viewmodel.FirstName,
                            LastName = viewmodel.LastName,
                            Email = viewmodel.Email,
                            Mobile = viewmodel.Mobile,
                            UserTypeId = 2,
                            MemberId = (Int32.Parse((from c in db.Users select c.MemberId).Max()) + 1).ToString()
                        };
                        db.Users.Add(user);
                        db.SaveChanges();

                        UsersLogin userlogin = new UsersLogin()
                        {
                            UserID = user.UserID,
                            Password = viewmodel.Password,
                            CreatedBy = user.UserID,
                            CreatedDate = DateTime.Now
                        };
                        db.UsersLogins.Add(userlogin);
                        db.SaveChanges();

                        Verification ver = new Verification()
                        {
                            UserId = user.UserID,
                            VerificationCode = viewmodel.VerificationCodeTemp,
                            IsMobileVerified = false,
                            IsEmailVerified = false
                        };
                        db.Verifications.Add(ver);
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["UserId"] = user.UserID;
                        viewmodel.VerificationCodeTemp = null;
                        viewmodel.resubmit = true;
                    }
                    catch (Exception e)
                    {
                        viewmodel.StatusCode = 4;
                        viewmodel.StatusMessage = e.Message;
                        dbTran.Rollback();
                    }
                }
            }
            //return RedirectToAction("Index", "Home");
            if (viewmodel.VerificationCode != null)
            {
                try
                {
                    Verification ver = new Verification();
                    int userid = (int)TempData["UserId"];
                    var objectVerification = (from u in db.Verifications
                                              where u.UserId == userid
                                              select new
                                              {
                                                  u.UserId,
                                                  u.VerificationCode,
                                                  u.IsEmailVerified,
                                                  u.IsMobileVerified,
                                                  u.MobileVerifyDate,
                                                  u.EmailVerifyDate,
                                                  u.VerificationId
                                              }).FirstOrDefault();
                    if (objectVerification != null)
                    {
                        ver.VerificationId = objectVerification.VerificationId;
                        ver.UserId = objectVerification.UserId;
                        ver.VerificationCode = objectVerification.VerificationCode;
                        ver.IsEmailVerified = objectVerification.IsEmailVerified;
                        ver.IsMobileVerified = objectVerification.IsMobileVerified;
                        ver.MobileVerifyDate = objectVerification.MobileVerifyDate;
                        ver.EmailVerifyDate = objectVerification.EmailVerifyDate;
                    }
                    if (ver != null)
                    {
                        if (ver.VerificationCode == viewmodel.VerificationCode)
                        {
                            ver.IsMobileVerified = true;
                            ver.MobileVerifyDate = DateTime.Now;
                            db.Entry(ver).State = EntityState.Modified;
                            db.SaveChanges();
                            viewmodel.StatusCode = 6;
                            viewmodel.StatusMessage = "Mobile verified!!!";
                            return RedirectToAction("Index", "Login");
                        }
                        else
                        {
                            viewmodel.StatusCode = 5;
                            viewmodel.StatusMessage = "Wrong Verification code entered.";
                        }
                    }
                }
                catch (Exception er)
                {
                    viewmodel.StatusCode = 4;
                    viewmodel.StatusMessage = er.Message;
                }
            }
            return View(viewmodel);
        }

        internal string SendSMS(VMUser user)
        {
            string number = "91" + user.Mobile;
            string passCode = RandomString(6);
            String message = HttpUtility.UrlEncode("Welcome to Yarta Coorg! Your verification code is-"+ passCode);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues(ConfigurationManager.AppSettings["SmsApiUrl"], new NameValueCollection()
                {
                {"apikey" , ConfigurationManager.AppSettings["SmsApiKey"]},
                {"numbers" , number},
                {"message" , message},
                {"sender" , ConfigurationManager.AppSettings["SmsSenderKey"]}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                user.VerificationCodeTemp = passCode;
                return result;
            }
        }

        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult OpenDashboard()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index","Login");
        }
    }
    
}