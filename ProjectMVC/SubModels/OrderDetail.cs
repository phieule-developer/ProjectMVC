using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectMVC.SubModels
{
     public class OrderDetail
     {
          public int ID_Order_Detail { get; set; }

          public string NameProduct { get; set; }

          public decimal? Price { get; set; }

          public int? Amount { get; set; }
          public string Color { get; set; }
          public string Size { get; set; }

          public int? ID_Order { get; set; }
     }
}