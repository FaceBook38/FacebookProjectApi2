namespace FaceBookAPI.Models.FaceBook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            Group_Members = new HashSet<Group_Members>();
            Group_Posts = new HashSet<Group_Posts>();
        }

        [Key]
        public int group_id { get; set; }

        public int group_admin { get; set; }

        [Required]
        [StringLength(50)]
        public string group_name { get; set; }

        [Column(TypeName = "image")]
        public byte[] group_image { get; set; }

        public bool? deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group_Members> Group_Members { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group_Posts> Group_Posts { get; set; }

        public virtual User User { get; set; }
    }
}
