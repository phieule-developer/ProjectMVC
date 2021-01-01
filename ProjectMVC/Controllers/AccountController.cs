using ProjectMVC.Models;
using ProjectMVC.SubModels;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectMVC.Controllers
{
    public class AccountController : Controller
    {
          // GET: Account
          DatabaseContext db = new DatabaseContext();
        public ActionResult Login()
        {
            return View();
        }
          [HttpPost]
          [Obsolete]
          public ActionResult LogIn(Member me,string checkBox)
          {
               try
               {
                    string password= FormsAuthentication.HashPasswordForStoringInConfigFile(me.Password, "MD5");

                    var member = db.Members.Where(m => m.Email == me.Email && m.Password == password).FirstOrDefault();
                    var admin = db.Admins.Where(m => m.Email == me.Email && m.Password == password).FirstOrDefault();

                    if(member != null )
                    {
                              SubMember sub = new SubMember();
                              sub.Email = member.Email;
                              sub.ID_Member = member.ID_Member;
                              sub.Name_Member = member.Name_Member;
                              sub.Phone_Member = member.Phone_Member;

                              if (checkBox == "on")
                              {
                                   Xcookie.Instance.SetMember(sub);
                              }
                              else
                              {
                                   Xsession.Member = sub;
                              }
                              if(Xsession.url == null)
                         {
                              Xsession.url = "Home";
                         }
                              return RedirectToAction("Index", Xsession.url);
                    }
                    else
                    {
                         ViewBag.message = "Tên tài khoản hoặc mật khẩu không chính xác";
                         return View();
                    }
               }
               catch (Exception e)
               {

                    return RedirectToAction("Login", "Account");
               }
          }
          public ActionResult SignUp()
          {
               return View();
          }
          [HttpPost]
          public ActionResult SignUp(Member me,string confirmPassword)
          {
               try
               {
                    var checkEmail = db.Members.Where(m => m.Email == me.Email).FirstOrDefault();
                    if (me.Password == confirmPassword)
                    {
                         if (checkEmail == null)
                         {
                              me.ID_Member = "" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                              me.Created_At = DateTime.Now;
                              me.Status = 1;
                              db.Members.Add(me);
                              db.SaveChanges();
                              return RedirectToAction("Index", "Home");
                         }
                         else
                         {
                              ViewBag.message = "Email đã tồn tại trong hệ thống";
                              return View();
                         }
                    }
                    else
                    {
                         ViewBag.error = "Mật khẩu và xác thực mật khẩu không chính xác";
                         return View();
                    }
               }
               catch (Exception)
               {
                    ViewBag.error = "Có lỗi xảy ra ^^";
                    return View();
               }
               
              
          }
          public ActionResult LogOut()
          {
               Xsession.RemoveAdmin();
               Xsession.RemoveMember();
               Xsession.url = null;
               Xcookie.Instance.RemoveMember();
               Xcookie.Instance.RemoveAdmin();
               return RedirectToAction("Login", "Account");
          }
     }
}