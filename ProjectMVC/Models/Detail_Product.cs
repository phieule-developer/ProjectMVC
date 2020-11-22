namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Detail_Product
    {
        [Key]
        public int ID_Detail_Product { get; set; }

        public int? ID_Product { get; set; }

        public int? Size_Product { get; set; }

        public int? Color_Name { get; set; }

        public int? Amount { get; set; }

        public virtual Color Color { get; set; }

        public virtual Product Product { get; set; }

        public virtual SizeProduct SizeProduct { get; set; }
    }
}
