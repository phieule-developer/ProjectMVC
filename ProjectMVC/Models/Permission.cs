namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Permission")]
    public partial class Permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Permission()
        {
            Detail_Per = new HashSet<Detail_Per>();
        }

        [Key]
        public int ID_Permission { get; set; }

        [StringLength(50)]
        public string Name_Permission { get; set; }

        [StringLength(50)]
        public string Key_Permission { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Per> Detail_Per { get; set; }
    }
}
