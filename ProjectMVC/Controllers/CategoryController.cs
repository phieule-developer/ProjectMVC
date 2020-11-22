using ProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMVC.SubModels;
using Product = ProjectMVC.SubModels.Product;

namespace ProjectMVC.Controllers
{
    public class CategoryController : BaseController
    {
          // GET: Category
          DatabaseContext db = new DatabaseContext();
          public ActionResult Index()
          {
               var productList = (from p in db.Products
                                 join c in db.Categories on p.ID_Category equals c.ID_Category
                                 where c.Sex == true
                                 select new Product
                                 {
                                      ID_Product = p.ID_Product,
                                      Name_Product = p.Name_Product,
                                      Image = p.Image,
                                      Current_Price = p.Current_Price,
                                      Name_Category = c.Name_Category
                                 }).ToList();
                                 
               ViewBag.productList = productList;
               return View();
        }
          public ActionResult categoryWoman()
          {
               var productList = (from p in db.Products
                                 join c in db.Categories on p.ID_Category equals c.ID_Category
                                 where c.Sex == false
                                 select new Product
                                 {
                                      ID_Product = p.ID_Product,
                                      Name_Product = p.Name_Product,
                                      Image = p.Image,
                                      Current_Price = p.Current_Price,
                                      Name_Category = c.Name_Category
                                 }).ToList();

             
               ViewBag.productList = productList;
               return View();
          }

          public JsonResult findAscendCategory(int category_id)
          {

               var productList = (from p in db.Products
                                  where p.ID_Category== category_id
                                  orderby p.Current_Price
                                  ascending
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });
              

               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          
          public JsonResult findDesendCategory(int category_id)
          {
               var productList = (from p in db.Products
                                  where p.ID_Category== category_id
                                  orderby p.Current_Price
                                  descending
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });


               return Json(productList, JsonRequestBehavior.AllowGet);
          }
         
          public JsonResult findCategoryMan(int category_id)
          {
               var productList = (from p in db.Products
                                  join c in db.Categories on p.ID_Category equals c.ID_Category
                                  where c.Sex == true && c.ID_Category == category_id
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });


               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          public JsonResult findCategoryWoMan(int category_id)
          {
               var productList = (from p in db.Products
                                  join c in db.Categories on p.ID_Category equals c.ID_Category
                                  where c.Sex == false && c.ID_Category == category_id
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });


               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          public JsonResult acsendCategoryMan()
          {
              
                  var productList = (from p in db.Products
                                   join c in db.Categories on p.ID_Category equals c.ID_Category
                                   where c.Sex == true
                                   orderby p.Current_Price
                                   ascending
                                   select new
                                   {
                                        Name_Product = p.Name_Product,
                                        Price = p.Current_Price,
                                        ID_Product = p.ID_Product,
                                        Image = p.Image
                                   });

               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          public JsonResult acsendCategoryWoMan()
          {

               var productList = (from p in db.Products
                                  join c in db.Categories on p.ID_Category equals c.ID_Category
                                  where c.Sex == false
                                  orderby p.Current_Price
                                  ascending
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });

               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          public JsonResult descendCategoryMan()
          {

               var productList = (from p in db.Products
                                  join c in db.Categories on p.ID_Category equals c.ID_Category
                                  where c.Sex == true
                                  orderby p.Current_Price
                                  descending
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });

               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          public JsonResult descendCategoryWoMan()
          {

               var productList = (from p in db.Products
                                  join c in db.Categories on p.ID_Category equals c.ID_Category
                                  where c.Sex == false
                                  orderby p.Current_Price
                                  descending
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });

               return Json(productList, JsonRequestBehavior.AllowGet);
          }
          public JsonResult Category(bool sex)
          {

               var productList = (from p in db.Products
                                  join c in db.Categories on p.ID_Category equals c.ID_Category
                                  where c.Sex == sex
                                  
                                  select new
                                  {
                                       Name_Product = p.Name_Product,
                                       Price = p.Current_Price,
                                       ID_Product = p.ID_Product,
                                       Image = p.Image
                                  });

               return Json(productList, JsonRequestBehavior.AllowGet);
          }

     }
}