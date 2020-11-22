using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMVC.Utils;

namespace ProjectMVC.Controllers
{
    public class BaseController : Controller
    {
          // GET: Base
          protected override void OnActionExecuting(ActionExecutingContext filterContext)
          {
               if (Xsession.Member == null)
               {
                    if (Xcookie.Instance.GetMember() != null)
                         Xsession.Member = Xcookie.Instance.GetMember();
               }
               base.OnActionExecuting(filterContext);
          }
     }
}