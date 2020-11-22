namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("District")]
    public partial class District
    {
        [Key]
        public int ID_District { get; set; }

        [StringLength(50)]
        public string Name_District { get; set; }

        public int? ID_Province { get; set; }

        public virtual Province Province { get; set; }
    }
}
