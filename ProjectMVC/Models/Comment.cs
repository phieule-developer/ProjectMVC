namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int ID_Comment { get; set; }

        public string Content_Comment { get; set; }

        public int? ID_Product { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime? Date_Comment { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public virtual Product Product { get; set; }
    }
}
