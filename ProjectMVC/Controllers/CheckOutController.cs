using Newtonsoft.Json;
using ProjectMVC.Models;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProjectMVC.Controllers
{
    public class CheckOutController : BaseController
    {
          // GET: CheckOut
          DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
               if(Xsession.Member == null)
               {
                    return RedirectToAction("Login", "Account");
               }
               else
               {
                    var province = (from p in db.Provinces
                                    select p).ToList();
                    ViewBag.province = province;
                    return View();
               }
            
        }
          [HttpPost]
          public ActionResult Payment(Order me, int province_id,string cart)
          {
               try
               {
                    if (me.Name == null || me.Address_Detail == null || me.Address_District == null || me.Email == null || me.Phone == null)
                    {
                         return RedirectToAction("Index", "CheckOut");
                    }
                    else
                    {
                         me.ID_Member = Xsession.Member.ID_Member;
                         me.Date_Order = DateTime.Now;
                         me.Address_Province = db.Provinces.Where(p => p.ID_Province == province_id).FirstOrDefault().Name_Province;
                         me.Status = 0;
                         db.Orders.Add(me);
                         db.SaveChanges();

                         List<Order_Detail> list = JsonConvert.DeserializeObject<List<Order_Detail>>(cart);
                         foreach (var i in list)
                         {
                              i.ID_Order = me.ID_Order;
                              db.Order_Detail.Add(i);
                              db.SaveChanges();
                         }
                         return RedirectToAction("Detail_Order", new { id_order = me.ID_Order });
                    }
               }
               catch (Exception)
               {

                    return RedirectToAction("Index", "CheckOut");
               }
               
          }
          public ActionResult Detail_Order(int id_order)
          {
               try
               {

                    var detailOrder = (from dp in db.Order_Detail
                                       join p in db.Products on dp.Id equals p.ID_Product
                                       where dp.ID_Order == id_order
                                       select new ProjectMVC.SubModels.Product{
                                            Name_Product = p.Name_Product,
                                            Current_Price= dp.Price,
                                            Color = dp.Color,
                                            Size = dp.Size,
                                            Amount = dp.Amount
                                       }).ToList();
                    if (detailOrder != null)
                    {
                         var order = (from d in db.Orders
                                       where d.ID_Order == id_order
                                       select d).ToList();
                         ViewBag.order = order;
                         ViewBag.detailOrder = detailOrder;
                         return View();
                    }
                    else
                    {
                         return RedirectToAction("Index", "Home");
                    }

                         
               }
               catch (Exception)
               {

                    return RedirectToAction("Index", "CheckOut");
               }
               
          }
          public JsonResult District(string id_province)// load các huyện với tính tương ứng
          {
               int ID_Province = int.Parse(id_province);
               var district = (from d in db.Districts
                               where d.ID_Province == ID_Province
                               select new
                               {
                                    Name_District = d.Name_District
                               }).ToList();
               return Json(district, JsonRequestBehavior.AllowGet);
          }
     }
}
