using ProjectMVC.Models;
using ProjectMVC.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
          // GET: Admin/Order
          DatabaseContext db = new DatabaseContext();
          public ActionResult Index()
        {
               var listOrder = db.Orders.OrderByDescending(p=>p.Date_Order).ToList();
               ViewBag.listOrder = listOrder;
            return View();
        }
          public ActionResult Accept(int id)
          {
               try
               {
                    var order = db.Orders.Where(p => p.ID_Order == id).FirstOrDefault();
                    order.Date_Process = DateTime.Now;
                    order.Status = 1;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Order", new { area = "Admin" });
               }
               catch (Exception)
               {

                    return RedirectToAction("Index", "Order", new { area = "Admin" });
               }
               
          }
          public ActionResult OrderDetail(int id)
          {
               try
               {
                    var orderDetail = (from p in db.Products
                                       join od in db.Order_Detail on p.ID_Product equals od.Id
                                       where od.ID_Order == id
                                       select new OrderDetail 
                                       {
                                            ID_Order= od.ID_Order,
                                            NameProduct= p.Name_Product,
                                            Amount= od.Amount,
                                            Price = od.Price,
                                            Color = od.Color,
                                            Size = od.Size
                                       }
                                       ).ToList();
                    ViewBag.orderDetail = orderDetail;
                    return View();
               }
               catch (Exception)
               {

                    return RedirectToAction("Index", "Order", new { area = "Admin" }); ;
               }
          }
    }
}