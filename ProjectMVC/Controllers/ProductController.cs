using ProjectMVC.Models;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Controllers
{
    public class ProductController : BaseController
    {
          DatabaseContext db = new DatabaseContext();
        public ActionResult Index(int id)
        {
               try
               {
                    var product = db.Products.Where(p => p.ID_Product == id).FirstOrDefault();
                    var comment = db.Comments.Where(c => c.ID_Product == id).OrderBy(p => p.Date_Comment).ToList();
                    if(product != null)
                    {
                         var images = db.Images.Where(i => i.ID_Product == id).ToList();
                         
                         ViewBag.product = product;
                         ViewBag.images = images;
                         ViewBag.comment = comment;
                         return View();
                    }
                    return RedirectToAction("Index", "Home");

               }
               catch
               {
                    return RedirectToAction("Index","Home");
               }
               
        }
     public JsonResult checkAmount(int id,string size,string color,int amount)
    {
               int color_id = db.Colors.Where(c => c.Name_Color == color).FirstOrDefault().ID_Color;
               int size_id = db.SizeProducts.Where(s => s.Size_Product == size).FirstOrDefault().ID_Size;

               var result = db.Detail_Product.Where(p => p.ID_Product == id && p.Size_Product == size_id && p.Color_Name == color_id && p.Amount >= amount).FirstOrDefault();
               
               if(result == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
               return Json(true, JsonRequestBehavior.AllowGet);
    }
          [HttpPost]
          public ActionResult Comment(string message,string id_product)
          {
               if(Xsession.Member == null)
               {
                 
                    return RedirectToAction("LogIn", "Account");
               }
               else
               {
                    Comment co = new Comment();
                    co.Content_Comment = message;
                    co.ID_Product =int.Parse(id_product);
                    co.Date_Comment = DateTime.Now;
                    co.Name = Xsession.Member.Name_Member;

                    db.Comments.Add(co);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = co.ID_Product });
               }
               
          }
    }
}