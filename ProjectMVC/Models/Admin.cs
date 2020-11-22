namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admin()
        {
            Detail_Per = new HashSet<Detail_Per>();
        }

        [Key]
        [StringLength(50)]
        public string ID_Admin { get; set; }

        [StringLength(50)]
        public string Name_Admin { get; set; }

        [StringLength(50)]
        public string Phone_Admin { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string Password { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Per> Detail_Per { get; set; }
    }
}
