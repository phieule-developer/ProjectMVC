using Newtonsoft.Json;
using ProjectMVC.Models;
using ProjectMVC.SubModels;
using ProjectMVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC.Controllers
{
    public class HomeController : BaseController
    {
          // GET: Home
          DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
               //string url ="https://raw.githubusercontent.com/sunshine-tech/VietnamProvinces/master/vietnam_provinces/data/nested-divisions.json";

               //var t = Task.Run(() => GetURI(new Uri(url)));
               //t.Wait();

               //var city = t.Result;
               //List<City> list = JsonConvert.DeserializeObject<List<City>>(city.ToString()); 
               //Ward pr;
               //foreach (var i in list)
               //{
               //     List<Dis> dis = JsonConvert.DeserializeObject<List<Dis>>(i.districts.ToString());
               //     foreach (var j in dis)
               //     {
               //          List<War> wa = JsonConvert.DeserializeObject<List<War>>(j.wards.ToString());
               //          foreach (var k in wa)
               //          {
               //               pr = new Ward();
               //               pr.ID_District = int.Parse(j.code);
               //               pr.Name_Ward = k.name;
               //               pr.ID_Ward = int.Parse(k.code);
               //               db.Wards.Add(pr);
               //               db.SaveChanges();
               //          }
               //     }    
               //}
               var product = db.Products.Where(p=>p.status != -1).Take(12).ToList();
               ViewBag.listProduct = product;
               return View();
        }
          async Task<Object> GetURI(Uri u)
          {
               var response = string.Empty;
               using (var client = new HttpClient())
               {
                    HttpResponseMessage result = await client.GetAsync(u);
                    if (result.IsSuccessStatusCode)
                    {
                         response = await result.Content.ReadAsStringAsync();
                    }
               }
               return response;
          }
     }
}