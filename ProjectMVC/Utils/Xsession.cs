using ProjectMVC.Models;
using ProjectMVC.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC.Utils
{
     public class Xsession
     {
          public static SubMember Member
          {
               get
               {
                    return HttpContext.Current.Session["Member"] as SubMember;
               }
               set
               {
                    HttpContext.Current.Session["Member"] = value;
               }
          }
          public static void RemoveMember()
          {
               HttpContext.Current.Session.Remove("Member");
          }
          public static SubAdmin Admin
          {
               get
               {
                    return HttpContext.Current.Session["Admin"] as SubAdmin;
               }
               set
               {
                    HttpContext.Current.Session["Admin"] = value;
               }
          }
          public static string url
          {
               get
               {
                    return HttpContext.Current.Session["controller"] as string;
               }
               set
               {
                    HttpContext.Current.Session["controller"] = value;
               }
          }
          public static void RemoveAdmin()
          {
               HttpContext.Current.Session.Remove("Admin");
          }
     }
}