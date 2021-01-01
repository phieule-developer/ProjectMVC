namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ward")]
    public partial class Ward
    {
        public int ID { get; set; }

        public int ID_Ward { get; set; }

        [StringLength(200)]
        public string Name_Ward { get; set; }

        public int? ID_District { get; set; }
    }
}
