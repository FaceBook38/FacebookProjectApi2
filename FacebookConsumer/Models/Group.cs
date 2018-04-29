using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FacebookConsumer.Models
{
    public class Group
    {
        [Key]
        public int group_id { get; set; }

        public int group_admin { get; set; }
        [Display(Name = "Group Name")]
        [Required]
        [StringLength(50)]
        public string group_name { get; set; }
        [Display(Name = "Image")]

        [Column(TypeName = "image")]
        public byte[] group_image { get; set; }

        public bool? deleted { get; set; }

        public virtual ICollection<Group_Members> Group_Members { get; set; }

        public virtual ICollection<Group_Posts> Group_Posts { get; set; }

        public virtual User User { get; set; }
    }
}
