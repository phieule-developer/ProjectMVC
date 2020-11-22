namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Description")]
    public partial class Description
    {
        public int ID { get; set; }

        public int? ID_Product { get; set; }

        [Column("Description")]
        public string Description1 { get; set; }

        public virtual Product Product { get; set; }
    }
}
