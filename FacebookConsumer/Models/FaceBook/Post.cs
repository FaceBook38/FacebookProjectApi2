using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacebookConsumer.Models.FaceBook
{
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        [Key]
        public int post_id { get; set; }

        public int user_id { get; set; }

        [StringLength(500)]
        public string content { get; set; }

        public bool? deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual User User { get; set; }
    }
}
