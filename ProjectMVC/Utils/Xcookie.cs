using Newtonsoft.Json;
using ProjectMVC.Models;
using ProjectMVC.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ProjectMVC.Utils
{
     public class Xcookie
     {
          private static Xcookie instance;
          public static Xcookie Instance
          {
               get
               {
                    if (instance == null)
                         return new Xcookie();
                    else return instance;
               }
               set { instance = value; }
          }
          private static HttpContext _context
          {
               get
               { return HttpContext.Current; }
          }
          /// <summary>
          /// member
          /// </summary>
          /// <param name="member"></param>
          public void SetMember(SubMember member)
          {
               
               string userData = JsonConvert.SerializeObject(member);
               FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, member.Name_Member, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
               string enTicket = FormsAuthentication.Encrypt(authTicket);
               HttpCookie usercookie = new HttpCookie("cookieMember", enTicket);
               usercookie.Expires = DateTime.Now.AddYears(1);
               usercookie.HttpOnly = true;
               _context.Response.SetCookie(usercookie);
          }

          public SubMember GetMember()
          {

               HttpCookie usecookie = _context.Request.Cookies["cookieMember"];
               if (usecookie != null)
               {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(usecookie.Value);
                    return JsonConvert.DeserializeObject<SubMember>(ticket.UserData);
               }

               return null;
          }

          public void RemoveMember()
          {
               if (_context.Response.Cookies["cookieMember"] != null)
               {
                    _context.Response.Cookies["cookieMember"].Value = null;
                    _context.Response.Cookies["cookieMember"].Expires = DateTime.Now.AddYears(-1);
               }
          }

          /// <summary>
          /// Admin
          /// </summary>
          /// <param name="member"></param>
          /// 


          public void SetAdmin(SubAdmin Admin)
          {
               string AdminData = JsonConvert.SerializeObject(Admin);
               FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                   (
                   1, Admin.Name_Admin, DateTime.Now, DateTime.Now.AddMinutes(15), false, AdminData
                   );

               string enTicket = FormsAuthentication.Encrypt(authTicket);

               HttpCookie Admincookie = new HttpCookie("cookieAdmin", enTicket);
               Admincookie.Expires = DateTime.Now.AddYears(1);
               Admincookie.HttpOnly = true;
               _context.Response.SetCookie(Admincookie);
          }

          public SubAdmin GetAdminCookie()
          {
               HttpCookie adminCookie = _context.Request.Cookies["cookieAdmin"];
               if (adminCookie != null)
               {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(adminCookie.Value);
                    return JsonConvert.DeserializeObject<SubAdmin>(ticket.UserData);
               }

               return null;
          }

          public void RemoveAdmin()
          {
               if (_context.Response.Cookies["cookieAdmin"] != null)
               {
                    _context.Response.Cookies["cookieAdmin"].Value = null;
                    _context.Response.Cookies["cookieAdmin"].Expires = DateTime.Now.AddYears(-1);
               }
          }
     }
}