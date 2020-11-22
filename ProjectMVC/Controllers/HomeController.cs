using ProjectMVC.Models;
using ProjectMVC.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Controllers
{
    public class HomeController : BaseController
    {
          // GET: Home
          DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
               var product = db.Products.Take(12).ToList();
               ViewBag.listProduct = product;
               return View();
        }
    }
}