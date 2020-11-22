using ProjectMVC.CustomAuthorizeAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        [CustomAuthorize(Name_Permission = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

          
    }
}