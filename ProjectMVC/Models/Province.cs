namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Province")]
    public partial class Province
    {
        public int ID { get; set; }

        public int ID_Province { get; set; }

        [StringLength(50)]
        public string Name_Province { get; set; }
    }
}
