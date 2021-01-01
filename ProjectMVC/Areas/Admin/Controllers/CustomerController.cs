using ProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
          // GET: Admin/Customer

          DatabaseContext db = new DatabaseContext();
          public ActionResult Index()
        {
               try
               {
                    var listMember = db.Members.ToList();
                    ViewBag.listMember = listMember;
                    return View();
               }
               catch (Exception)
               {

                    return View();
               }
               
        }
          public ActionResult Delete(string id)
          {
               try
               {
                    var member = db.Members.Where(m => m.ID_Member == id).FirstOrDefault();
                    member.Status = -1;
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }
               catch (Exception)
               {

                    return RedirectToAction("Index");
               }
               
          }
    }
}