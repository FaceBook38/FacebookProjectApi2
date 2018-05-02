using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacebookConsumer.Models.FaceBook
{
    public partial class Group_Posts
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int post_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int group_id { get; set; }
  
        public int user_id { get; set; }
        [StringLength(500)]
        public string content { get; set; }
        public bool? deleted { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
