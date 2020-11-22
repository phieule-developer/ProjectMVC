namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        public int? ID_Product { get; set; }

        public virtual Product Product { get; set; }
    }
}
