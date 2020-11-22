namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Detail_Per
    {
        [StringLength(50)]
        public string ID_Admin { get; set; }

        public int? ID_Permission { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Detail_Per { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
