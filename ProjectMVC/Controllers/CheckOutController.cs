using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectMVC.Models;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
                    Xsession.url = "CheckOut";
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
          public ActionResult Payment(Order me, int province_id,int district_id,string ward_id,string Address_Detail,int Payment,string cart)
          {
               try
               {
                    if (me.Name == null  || me.Email == null || me.Phone == null|| province_id == 0|| district_id == 0 || Address_Detail == null)
                    {
                         return RedirectToAction("Index", "CheckOut");
                    }
                    else
                    {
                         // status = 0 : Yêu cầu chờ thanh toán
                         // = 1 : Thanh toán tiền mặt
                         // = 2 : Thanh toán thành công
                         // = 3 : Thanh toán thất bại
                         decimal total = 0;
                         me.ID_Member = Xsession.Member.ID_Member;
                         me.Date_Order = DateTime.Now;
                         string province = db.Provinces.Where(p => p.ID_Province == province_id).FirstOrDefault().Name_Province;
                         string district = db.Districts.Where(p => p.ID_District == district_id).FirstOrDefault().Name_District;
                         me.Address = Address_Detail + " " + ward_id + " " + district + " " + province;
                         if(Payment == 1)
                         {
                              me.Status = 4;
                         }
                         else
                         {
                              me.Status = 0;
                         }
                         
                         db.Orders.Add(me);
                         db.SaveChanges();

                         List<Order_Detail> list = JsonConvert.DeserializeObject<List<Order_Detail>>(cart);
                         
                         foreach (var i in list)
                         {
                              i.ID_Order = me.ID_Order;
                              total += decimal.Parse(i.Price.ToString())*decimal.Parse(i.Amount.ToString());
                              db.Order_Detail.Add(i);
                              db.SaveChanges();
                         }

                         if (Payment == 1)
                         {
                              foreach (var i in list)
                              {
                                   i.ID_Order = me.ID_Order;
                                   
                                   int SIZE_ID = 0;
                                   int Color_ID = 0;
                                   if (i.Size == "M")
                                   {
                                        SIZE_ID = 5;
                                   }
                                   else if (i.Size == "L")
                                   {
                                        SIZE_ID = 6;
                                   }
                                   else if (i.Size == "XL")
                                   {
                                        SIZE_ID = 7;
                                   }
                                   else
                                   {
                                        SIZE_ID = 8;
                                   }
                                   if (i.Color == "Đỏ")
                                   {
                                        Color_ID = 1;
                                   }
                                   else if (i.Color == "Xanh")
                                   {
                                        Color_ID = 2;
                                   }
                                   else
                                   {
                                        Color_ID = 2;
                                   }
                                   var detail = db.Detail_Product.Where(p => p.ID_Product == i.Id && p.Size_Product == SIZE_ID && p.Color_Name == Color_ID).FirstOrDefault();
                                   detail.Amount -= i.Amount;
                                   db.SaveChanges();
                              }
                              return RedirectToAction("Detail_Order", new { id_order = me.ID_Order });
                         }
                         else
                         {
                              string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                              string partnerCode = "MOMOTZKV20200828";
                              string accessKey = "WRp4TCwfWinbalSx";
                              string serectkey = "IvbHewMnoUTt4midCbUlMUtVU8lRPBJe";
                              string orderInfo = "Thanh toán hóa đơn tại cửa hàng của Phiêu";
                              string returnUrl = "https://localhost:44334/CheckOut/Success";
                              string notifyurl = "https://localhost:44334/CheckOut/Update";

                              string amount = ""+total;
                              string orderid = me.ID_Order.ToString();
                              string requestId = Guid.NewGuid().ToString();
                              string extraData = "";

                              //Before sign HMAC SHA256 signature
                              string rawHash = "partnerCode=" +
                                  partnerCode + "&accessKey=" +
                                  accessKey + "&requestId=" +
                                  requestId + "&amount=" +
                                  amount + "&orderId=" +
                                  orderid + "&orderInfo=" +
                                  orderInfo + "&returnUrl=" +
                                  returnUrl + "&notifyUrl=" +
                                  notifyurl + "&extraData=" +
                                  extraData;


                              //sign signature SHA256
                              string signature = signSHA256(rawHash, serectkey);

                              //build body json request
                              JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

                              string responseFromMomo = sendPaymentRequest(endpoint, message.ToString());

                              JObject jmessage = JObject.Parse(responseFromMomo);

                              return Redirect(jmessage.GetValue("payUrl").ToString());
                         }
                         
                    }
               }
               catch (Exception)
               {

                    return RedirectToAction("Index", "CheckOut");
               }
               
          }

          public ActionResult Pay(int id_order)
          {
               try
               {


                         var order = (from d in db.Order_Detail
                                      where d.ID_Order == id_order
                                      select d).Sum(p=>p.Price * p.Amount);


                    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                    string partnerCode = "MOMOTZKV20200828";
                    string accessKey = "WRp4TCwfWinbalSx";
                    string serectkey = "IvbHewMnoUTt4midCbUlMUtVU8lRPBJe";
                    string orderInfo = ""+id_order;
                    string returnUrl = "https://localhost:44334/CheckOut/SuccessPay";
                    string notifyurl = "https://localhost:44334/CheckOut/UpdatePay";

                    string amount = "" + order;
                    string orderid = Guid.NewGuid().ToString();
                    string requestId = Guid.NewGuid().ToString();
                    string extraData = "";

                    //Before sign HMAC SHA256 signature
                    string rawHash = "partnerCode=" +
                        partnerCode + "&accessKey=" +
                        accessKey + "&requestId=" +
                        requestId + "&amount=" +
                        amount + "&orderId=" +
                        orderid + "&orderInfo=" +
                        orderInfo + "&returnUrl=" +
                        returnUrl + "&notifyUrl=" +
                        notifyurl + "&extraData=" +
                        extraData;


                    //sign signature SHA256
                    string signature = signSHA256(rawHash, serectkey);

                    //build body json request
                    JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

                    string responseFromMomo = sendPaymentRequest(endpoint, message.ToString());

                    JObject jmessage = JObject.Parse(responseFromMomo);

                    return Redirect(jmessage.GetValue("payUrl").ToString());


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
                                    ID_District = d.ID_District,
                                    Name_District = d.Name_District
                               }).ToList();
               return Json(district, JsonRequestBehavior.AllowGet);
          }
          public JsonResult Ward(string id_district)// load các huyện với tính tương ứng
          {
               int ID_District = int.Parse(id_district);
               var district = (from d in db.Wards
                               where d.ID_District == ID_District
                               select new
                               {
                                    Name_Ward = d.Name_Ward
                               }).ToList();
               return Json(district, JsonRequestBehavior.AllowGet);
          }
          public ActionResult Success()
          {
               string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
               param = Server.UrlDecode(param);
               string serectKey = "IvbHewMnoUTt4midCbUlMUtVU8lRPBJe";
               string signature = signSHA256(param, serectKey);
               var id_order = int.Parse(Request.QueryString["orderId"].ToString());
               var detailOrder = (from dp in db.Order_Detail
                                  join p in db.Products on dp.Id equals p.ID_Product
                                  where dp.ID_Order == id_order
                                  select new ProjectMVC.SubModels.Product
                                  {
                                       Name_Product = p.Name_Product,
                                       Current_Price = dp.Price,
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

               }
               if (signature != Request.QueryString["signature"].ToString())
               {
                    ViewBag.message = "Thông tin đơn hàng không hợp lệ";
                    var order = db.Orders.Where(o => o.ID_Order == id_order).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 3;
                         db.SaveChanges();
                    }
                    return View();
               }
               if (Request.QueryString["errorCode"].Equals("0"))
               {
                    var order = db.Orders.Where(o => o.ID_Order == id_order).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         var detail_order = db.Order_Detail.Where(p => p.ID_Order == id_order).ToList();

                         foreach(var de in detail_order)
                         {
                              int ID_Size=0,ID_Color=0;
                              if (de.Size == "M")
                              {
                                   ID_Size = 5;
                              }
                              else if (de.Size == "L")
                              {
                                   ID_Size = 6;
                              }
                              else if (de.Size == "XL")
                              {
                                   ID_Size = 8;
                              }
                              else if (de.Size == "XXL")
                              {
                                   ID_Size = 7;
                              }
                              if(de.Color == "Đỏ")
                              {
                                   ID_Color = 1;
                              }else if (de.Color == "Xanh")
                              {
                                   ID_Color = 2;
                              }
                              else if (de.Color == "Trắng")
                              {
                                   ID_Color = 3;
                              }
                              var detail = db.Detail_Product.Where(p => p.ID_Product == de.Id && p.Color_Name == ID_Color && p.Size_Product == ID_Size).FirstOrDefault();
                              detail.Amount -= de.Amount;
                              db.SaveChanges();
                         }
                         order.Status = 2;
                         db.SaveChanges();
                    }
                    ViewBag.message = "Thanh toán thành công";
                    
               }
               else
               {
                    var order = db.Orders.Where(o => o.ID_Order == id_order).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 3;
                         db.SaveChanges();
                    }
                    ViewBag.message = "Thanh toán thất bại";
               }
               return View();
          }
          public ActionResult SuccessPay()
          {
               string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
               param = Server.UrlDecode(param);
               string serectKey = "IvbHewMnoUTt4midCbUlMUtVU8lRPBJe";
               string signature = signSHA256(param, serectKey);
               var id_order = int.Parse(Request.QueryString["orderInfo"].ToString());
               var detailOrder = (from dp in db.Order_Detail
                                  join p in db.Products on dp.Id equals p.ID_Product
                                  where dp.ID_Order == id_order
                                  select new ProjectMVC.SubModels.Product
                                  {
                                       Name_Product = p.Name_Product,
                                       Current_Price = dp.Price,
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

               }
               if (signature != Request.QueryString["signature"].ToString())
               {
                    ViewBag.message = "Thông tin đơn hàng không hợp lệ";
                    var order = db.Orders.Where(o => o.ID_Order == id_order).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 3;
                         db.SaveChanges();
                    }
                    return View();
               }
               if (Request.QueryString["errorCode"].Equals("0"))
               {
                    var order = db.Orders.Where(o => o.ID_Order == id_order).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 2;
                         db.SaveChanges();
                    }
                    ViewBag.message = "Thanh toán thành công";

               }
               else
               {
                    var order = db.Orders.Where(o => o.ID_Order == id_order).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 3;
                         db.SaveChanges();
                    }
                    ViewBag.message = "Thanh toán thất bại";
               }
               return View();
          }
          public JsonResult Update()
          {

               string param = "partnerCode=" + Request["partner_code"] +
                    "&accessKey=" + Request["access_key"] +
                    "&requestId=" + Request["requestId"] +
                    "&amount=" + Request["amount"] +
                    "&orderId=" + Request["orderId"] +
                    "&orderInfo=" + Request["orderInfo"] +
                    "&orderType=" + Request["orderType"] +
                    "&transId=" + Request["transId"] +
                    "&message=" + Request["message"] +
                    "&responseTime=" + Request["responseTime"]+
                    "&errorCode=" + Request["errorCode"] +
                    "&payType=" + Request["payType"]+
                    "&extraData=" + Request["extraData"];

               param = Server.UrlDecode(param);
               string serectKey = "IvbHewMnoUTt4midCbUlMUtVU8lRPBJe";

               string signature = signSHA256(param, serectKey).ToString();
               int order_id =int.Parse(Request["orderId"].ToString());
               // status = 0 : Yêu cầu chờ thanh toán
               // = 1 : Thanh toán tiền mặt
               // = 2 : Thanh toán thành công
               // = 3 : Thanh toán thất bại
               if (signature != Request.QueryString["signature"].ToString())
               {
                    var order = db.Orders.Where(o => o.ID_Order == order_id).FirstOrDefault();
                    if(order.Status == 0)
                    {
                         order.Status = 3;
                         db.SaveChanges();
                    }
               }
               if (Request.QueryString["orderId"].Equals("0"))
               {
                    var order = db.Orders.Where(o => o.ID_Order == order_id).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 2;
                         db.SaveChanges();
                    }
               }
               else
               {
                    var order = db.Orders.Where(o => o.ID_Order == order_id).FirstOrDefault();
                    if (order.Status == 0)
                    {
                         order.Status = 3;
                         db.SaveChanges();
                    }
               }

               return Json("", JsonRequestBehavior.AllowGet);
          }
          public string getHash(string partnerCode, string merchantRefId,
            string amount, string paymentCode, string storeId, string storeName, string publicKeyXML)
          {
               string json = "{\"partnerCode\":\"" +
                   partnerCode + "\",\"partnerRefId\":\"" +
                   merchantRefId + "\",\"amount\":" +
                   amount + ",\"paymentCode\":\"" +
                   paymentCode + "\",\"storeId\":\"" +
                   storeId + "\",\"storeName\":\"" +
                   storeName + "\"}";

               byte[] data = Encoding.UTF8.GetBytes(json);
               string result = null;
               using (var rsa = new RSACryptoServiceProvider(4096)) //KeySize
               {
                    try
                    {
                         // MoMo's public key has format PEM.
                         // You must convert it to XML format. Recommend tool: https://superdry.apphb.com/tools/online-rsa-key-converter
                         rsa.FromXmlString(publicKeyXML);
                         var encryptedData = rsa.Encrypt(data, false);
                         var base64Encrypted = Convert.ToBase64String(encryptedData);
                         result = base64Encrypted;
                    }
                    finally
                    {
                         rsa.PersistKeyInCsp = false;
                    }

               }

               return result;

          }
          public string buildQueryHash(string partnerCode, string merchantRefId,
              string requestid, string publicKey)
          {
               string json = "{\"partnerCode\":\"" +
                   partnerCode + "\",\"partnerRefId\":\"" +
                   merchantRefId + "\",\"requestId\":\"" +
                   requestid + "\"}";

               byte[] data = Encoding.UTF8.GetBytes(json);
               string result = null;
               using (var rsa = new RSACryptoServiceProvider(2048))
               {
                    try
                    {
                         // client encrypting data with public key issued by server
                         rsa.FromXmlString(publicKey);
                         var encryptedData = rsa.Encrypt(data, false);
                         var base64Encrypted = Convert.ToBase64String(encryptedData);
                         result = base64Encrypted;
                    }
                    finally
                    {
                         rsa.PersistKeyInCsp = false;
                    }

               }

               return result;

          }

          public string buildRefundHash(string partnerCode, string merchantRefId,
              string momoTranId, long amount, string description, string publicKey)
          {
               string json = "{\"partnerCode\":\"" +
                   partnerCode + "\",\"partnerRefId\":\"" +
                   merchantRefId + "\",\"momoTransId\":\"" +
                   momoTranId + "\",\"amount\":" +
                   amount + ",\"description\":\"" +
                   description + "\"}";

               byte[] data = Encoding.UTF8.GetBytes(json);
               string result = null;
               using (var rsa = new RSACryptoServiceProvider(2048))
               {
                    try
                    {
                         // client encrypting data with public key issued by server
                         rsa.FromXmlString(publicKey);
                         var encryptedData = rsa.Encrypt(data, false);
                         var base64Encrypted = Convert.ToBase64String(encryptedData);
                         result = base64Encrypted;
                    }
                    finally
                    {
                         rsa.PersistKeyInCsp = false;
                    }

               }

               return result;

          }
          public string signSHA256(string message, string key)
          {
               byte[] keyByte = Encoding.UTF8.GetBytes(key);
               byte[] messageBytes = Encoding.UTF8.GetBytes(message);
               using (var hmacsha256 = new HMACSHA256(keyByte))
               {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    string hex = BitConverter.ToString(hashmessage);
                    hex = hex.Replace("-", "").ToLower();
                    return hex;

               }
          }
          public string sendPaymentRequest(string endpoint, string postJsonString)
          {

               try
               {
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                    var postData = postJsonString;

                    var data = Encoding.UTF8.GetBytes(postData);

                    httpWReq.ProtocolVersion = HttpVersion.Version11;
                    httpWReq.Method = "POST";
                    httpWReq.ContentType = "application/json";

                    httpWReq.ContentLength = data.Length;
                    httpWReq.ReadWriteTimeout = 30000;
                    httpWReq.Timeout = 30000;
                    Stream stream = httpWReq.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();

                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                    string jsonresponse = "";

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {

                         string temp = null;
                         while ((temp = reader.ReadLine()) != null)
                         {
                              jsonresponse += temp;
                         }
                    }


                    //todo parse it
                    return jsonresponse;
                    //return new MomoResponse(mtid, jsonresponse);

               }
               catch (WebException e)
               {
                    return e.Message;
               }
          }
     }
}
