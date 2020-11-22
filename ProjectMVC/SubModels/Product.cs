using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC.SubModels
{
     public class Product
     {

          public int ID_Product { get; set; }

          public string Image { get; set; }
          public string Name_Product { get; set; }

          public string Name_Category { get; set; }
          public decimal? Current_Price { get; set; }

          public string Color { get; set; }
          public string Size { get; set; }

          public int? Amount { get; set; }

     }
}