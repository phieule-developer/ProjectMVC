using ProjectMVC.CustomAuthorizeAttribute;
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
    public class AdminController : BaseController
    {
          DatabaseContext db = new DatabaseContext();
          public ActionResult LogIn(){
               return View();
        }
        
       [HttpPost]
       public ActionResult LogIn(Admin me, string checkBox){
               try
               {
                    string password = FormsAuthentication.HashPasswordForStoringInConfigFile(me.Password, "MD5");

                    var admin = db.Admins.Where(m => m.Email == me.Email && m.Password == password).FirstOrDefault();

                    if (admin != null)
                    {
                         SubAdmin sub = new SubAdmin();
                         sub.Email = admin.Email;
                         sub.ID_Admin = admin.ID_Admin;
                         sub.Name_Admin = admin.Name_Admin;
                         sub.Phone_Admin = admin.Phone_Admin;

                         if (checkBox == "on")
                         {
                              Xcookie.Instance.SetAdmin(sub);
                         }
                         else
                         {
                              Xsession.Admin = sub;
                         }
                         return RedirectToAction("Index", "Admin");
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
        [CustomAuthorize(Name_Permission = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}