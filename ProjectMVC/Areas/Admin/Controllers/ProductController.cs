using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.Sheets.v4.Data;
using ProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel; 
namespace ProjectMVC.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
          // GET: Admin/Product
          DatabaseContext db = new DatabaseContext();

          // Thêm sản phẩm
          public ActionResult Index()
        {
            return View();
        }
          // Lấy danh sách sản phẩm
          [HttpPost]
          public ActionResult Index(Product product, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3,int XanhM,int XanhL, int XanhXL, int XanhXXL, int DoM, int DoL, int DoXL, int DoXXL, int TrangM, int TrangL, int TrangXL, int TrangXXL)
          {
               try
               {
                    // THÊM VÀO BẢNG SẢN PHẨM
                    Image im2 = new Image();
                    Image im3 = new Image();
                    Image im1 = new Image();
                    if (image2 != null)
                    {
                         product.Image = "/Content/Products/" + image2.FileName;
                         image2.SaveAs(Server.MapPath("/Content/Products/" + image2.FileName));
                         im2.URL = "/Content/Products/" + image2.FileName;
                         
                    }
                    if (image3 != null)
                    {
                         product.Image = "/Content/Products/" + image3.FileName;
                         image3.SaveAs(Server.MapPath("/Content/Products/" + image3.FileName));
                         im3.URL = "/Content/Products/" + image3.FileName;
                         
                    }
                    if (image1 != null)
                    {
                         product.Image = "/Content/Products/" + image1.FileName;
                         image1.SaveAs(Server.MapPath("/Content/Products/" + image1.FileName));
                         im1.URL = "/Content/Products/" + image1.FileName;
                         
                    }

                    product.Date_Post = DateTime.Now;
                    product.status = 0;
                    db.Products.Add(product);

                    if (image2 != null)
                    {
                         im2.ID_Product = product.ID_Product;
                         db.Images.Add(im2);
                         db.SaveChanges();
                    }
                    if (image3 != null)
                    {
                         im3.ID_Product = product.ID_Product;
                         db.Images.Add(im3);
                         db.SaveChanges();
                    }
                    if (image1 != null)
                    {                   
                         im1.ID_Product = product.ID_Product;
                         db.Images.Add(im1);
                         db.SaveChanges();
                    }
                    Detail_Product detail =new Detail_Product();
                    detail.ID_Product = product.ID_Product;
                    
                    detail.Amount = XanhM;
                    detail.Color_Name = 1;
                    detail.Size_Product = 5;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = XanhL;
                    detail.Color_Name = 1;
                    detail.Size_Product = 6;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = XanhXL;
                    detail.Color_Name = 1;
                    detail.Size_Product = 7;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = XanhXXL;
                    detail.Color_Name = 1;
                    detail.Size_Product = 8;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();




                    detail.Amount = DoM;
                    detail.Color_Name = 2;
                    detail.Size_Product = 5;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = DoL;
                    detail.Color_Name = 2;
                    detail.Size_Product = 6;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = DoXL;
                    detail.Color_Name = 2;
                    detail.Size_Product = 7;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = DoXXL;
                    detail.Color_Name = 2;
                    detail.Size_Product = 8;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();



                    detail.Amount = TrangM;
                    detail.Color_Name = 3;
                    detail.Size_Product = 5;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = TrangL;
                    detail.Color_Name = 3;
                    detail.Size_Product = 6;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = TrangXL;
                    detail.Color_Name = 3;
                    detail.Size_Product = 7;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    detail.Amount = TrangXXL;
                    detail.Color_Name = 3;
                    detail.Size_Product = 8;
                    db.Detail_Product.Add(detail);
                    db.SaveChanges();

                    return View();
               }
               catch (Exception e)
               {

                    return View();
               }
               
          }
          public ActionResult ListProduct()
          {
               try
               {
                    var listProduct = db.Products.Where(p => p.status != -1).ToList();
                    ViewBag.listProduct = listProduct;
                    return View();
               }
               catch (Exception)
               {
                    return View();
               }
              
          }
          public ActionResult UpdateProduct(int id)
          {
               var product = db.Products.Where(p => p.ID_Product == id).FirstOrDefault();

               var listSize = db.Detail_Product.Where(p => p.ID_Product == id).ToList();

               ViewBag.product = product;
               ViewBag.listSize = listSize;
               return View();
          }
          [HttpPost]
          public ActionResult UpdateProduct(Product pt, int XanhM, int XanhL, int XanhXL, int XanhXXL, int DoM, int DoL, int DoXL, int DoXXL, int TrangM, int TrangL, int TrangXL, int TrangXXL)
          {
               try
               {
                    var product = db.Products.Where(p => p.ID_Product == pt.ID_Product).FirstOrDefault();
                    product.Name_Product = pt.Name_Product;
                    product.Description_Product = pt.Description_Product;
                    product.Current_Price = pt.Current_Price;

                    var detail_product= new Detail_Product();
                    
                    var colors = db.Colors.ToList();
                    var sizes = db.SizeProducts.ToList();
                         foreach(var co in colors)
                         {
                              foreach(var si in sizes)
                              {
                              detail_product = db.Detail_Product.Where(p => p.ID_Product == pt.ID_Product && p.Color_Name == co.ID_Color && p.Size_Product == si.ID_Size).FirstOrDefault();
                              if (si.Size_Product == "M" && co.Name_Color == "Đỏ")
                                   {
                                        
                                        detail_product.Amount = DoM;
                                  
                              }
                                   else if (si.Size_Product == "M" && co.Name_Color == "Xanh")
                                   {
                            
                                        detail_product.Amount = XanhM;
                                 
                              }
                                   else if (si.Size_Product == "M" && co.Name_Color == "Trắng")
                                   {
                                        detail_product.Amount = TrangM;
                              }
                                   else if (si.Size_Product == "L" && co.Name_Color == "Đỏ")
                                   {
                                        detail_product.Amount = DoL;
                              }
                                   else if (si.Size_Product == "L" && co.Name_Color == "Xanh")
                                   {
                                        detail_product.Amount = XanhL;
                              }
                                   else if (si.Size_Product == "L" && co.Name_Color == "Trắng")
                                   {
                                        detail_product.Amount = TrangL;
                              }
                                   else if (si.Size_Product == "XL" && co.Name_Color == "Đỏ")
                                   {
                                        detail_product.Amount = DoXL;
                              }
                                   else if (si.Size_Product == "XL" && co.Name_Color == "Xanh")
                                   {
                                        detail_product.Amount = XanhXL;
                              }
                                   else if (si.Size_Product == "XL" && co.Name_Color == "Trắng")
                                   {
                                        detail_product.Amount = TrangXL;
                              }
                                   else if (si.Size_Product == "XXL" && co.Name_Color == "Đỏ")
                                   {
                                        detail_product.Amount = DoXXL;
                              }
                                   else if (si.Size_Product == "XXL" && co.Name_Color == "Xanh")
                                   {
                                        detail_product.Amount = XanhXXL;
                              }
                                   else if (si.Size_Product == "XXL" && co.Name_Color == "Trắng")
                                   {
                                        detail_product.Amount = TrangXXL;
                              }
                              db.SaveChanges();    
                         }
                    }
                    return RedirectToAction("UpdateProduct", "Product", new { area = "Admin", id = pt.ID_Product });
               }
               catch (Exception e)
               {

                    return RedirectToAction("UpdateProduct", "Product", new { area = "Admin", id = pt.ID_Product });
               }
               
          }
          public ActionResult delete(int id)
          {
               try
               {
                    var product = db.Products.Where(p => p.ID_Product == id).FirstOrDefault();
                    product.status = -1;
                    db.SaveChanges();
                    return RedirectToAction("ListProduct");
               }
               catch (Exception)
               {

                    return RedirectToAction("ListProduct");
               }
          }
          public ActionResult detail(int id)
          {
               try
               {
                    var product = db.Products.Where(p => p.ID_Product == id).FirstOrDefault();
                    ViewBag.product = product;
                    return RedirectToAction("ListProduct");
               }
               catch (Exception)
               {
                    return RedirectToAction("ListProduct");
               }
          }
          public ActionResult Import(int id)
          {

               return View();
          }
          

     }
}