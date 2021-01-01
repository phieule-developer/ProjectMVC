using ProjectMVC.CustomAuthorizeAttribute;
using ProjectMVC.Models;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
          DatabaseContext db = new DatabaseContext();
          // GET: Admin/Home
          [CustomAuthorize(Name_Permission = "Admin")]
        public ActionResult Index()
        {
               try
               {
                    
                    var countProduct = db.Products.Count(p=>p.status  != -1);
                    var countCustomer = db.Members.Count(p => p.Status != -1);
                    var countOrder = db.Orders.Count();
                    ViewBag.countOrder = countOrder;
                    ViewBag.countProduct = countProduct;
                    ViewBag.countCustomer = countCustomer;
                    return View();
               }
               catch (Exception)
               {
                    return View();
               }
               
        }

          
    }
}