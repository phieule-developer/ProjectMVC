namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order_Detail
    {
        [Key]
        public int ID_Order_Detail { get; set; }

        public int? Id { get; set; }

        public decimal? Price { get; set; }

        public int? Amount { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        public int? ID_Order { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
