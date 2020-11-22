namespace ProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [StringLength(50)]
        public string ID_Member { get; set; }

        [StringLength(50)]
        public string Name_Member { get; set; }

        [StringLength(50)]
        public string Phone_Member { get; set; }

        public string Password { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime? Created_At { get; set; }

        public int? Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
