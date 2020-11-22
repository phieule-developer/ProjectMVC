using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectMVC.SubModels
{
     public class SubAdmin
     {
          
          [StringLength(50)]
          public string ID_Admin { get; set; }

          [StringLength(50)]
          public string Name_Admin { get; set; }

          [StringLength(50)]
          public string Phone_Admin { get; set; }

          [StringLength(50)]
          public string Email { get; set; }

          public string Address { get; set; }
     }
}