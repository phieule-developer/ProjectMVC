using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectMVC.SubModels
{
     public class SubMember
     {
          public string ID_Member { get; set; }
          public string Name_Member { get; set; }

          public string Phone_Member { get; set; }
       
          public string Email { get; set; }

     }
}