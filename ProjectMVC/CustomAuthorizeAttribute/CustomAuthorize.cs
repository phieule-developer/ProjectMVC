
using ProjectMVC.Models;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.CustomAuthorizeAttribute
{
     public class CustomAuthorize: AuthorizeAttribute
     {
          DatabaseContext db = new DatabaseContext();
          public string Name_Permission;
          protected override bool AuthorizeCore(HttpContextBase httpContext)
          {
               if (Xsession.Admin == null)
               {
                    if (Xcookie.Instance.GetAdminCookie() != null)
                    {
                         Xsession.Admin = Xcookie.Instance.GetAdminCookie();
                    }
               }
               if (Xsession.Admin != null)
               {
                    string[] Permission = Name_Permission.Split('|');
                    var ID_Admin = Xsession.Admin.ID_Admin;
                    var list_ID_Per = db.Detail_Per.Where(o => o.ID_Admin == ID_Admin).Select(o => o.ID_Permission).ToList();
                    foreach (var item in list_ID_Per)
                    {
                         foreach (var per in Permission)
                         {
                              if (db.Permissions.Where(o => o.ID_Permission == item).FirstOrDefault().Key_Permission == per)
                                   return true;
                         }
                    }

               }
               return false;
          }
          protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
          {
               filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Index" }, { "controller", "Home" }, { "area", null } });
          }
     }
}