using ProjectMVC.Models;
using ProjectMVC.SubModels;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Controllers
{
    public class TrackingController : BaseController
    {
          // GET: Tracking
          DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
               if (Xsession.Member == null)
               {
                    Xsession.url = "Tracking";
                    return RedirectToAction("Login", "Account");
               }
               else
               {
                    var tracking = db.Orders.Where(o => o.ID_Member == Xsession.Member.ID_Member).OrderByDescending(p=>p.ID_Order).ToList();
                    ViewBag.tracking = tracking;
                    return View();
               }
        }
          public ActionResult Order(int id)
          {
               var checkOrder = db.Orders.Where(p => p.ID_Order == id).FirstOrDefault();
               if(checkOrder ==  null)
               {
                    return View();
               }
               else
               {
                    if (checkOrder.ID_Member == Xsession.Member.ID_Member)
                    {
                         var order = (from o in db.Order_Detail
                                      join p in db.Products on o.Id equals p.ID_Product
                                      where o.ID_Order == id
                                      select new OrderDetail
                                      {
                                           Amount = o.Amount,
                                           Color = o.Color,
                                           ID_Order = o.ID_Order,
                                           NameProduct = p.Name_Product,
                                           Price = o.Price,
                                           Size = o.Size
                                      }
                           ).ToList();
                         ViewBag.order = order;
                         return View();
                    }
                    else
                    {
                         return RedirectToAction("Index");
                    }
                    
               }
               
          }
          public ActionResult cancelOrder(int id)
          {
               try
               {
                    var checkOrder = db.Orders.Where(p => p.ID_Order == id && p.ID_Member == Xsession.Member.ID_Member).FirstOrDefault();

                    if (checkOrder != null)
                    {
                         if (checkOrder.Status != 0)
                         {
                              ViewBag.message = "Không thể xóa đơn hàng này";
                         }
                         else
                         {
                              checkOrder.Status = -1;
                              ViewBag.message = "Hủy đơn hàng thành công";
                              db.SaveChanges();
                         }
                         return RedirectToAction("Index");
                    }
                    else
                    {
                         ViewBag.message = "Không tồn tại đơn hàng này hoặc do bạn không có đơn hàng này";
                         return RedirectToAction("Index");
                    }

               }
               catch (Exception)
               {

                    return RedirectToAction("Index");
               }
              
          }
    }
}