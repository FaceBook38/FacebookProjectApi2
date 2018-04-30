namespace FacebookConsumer.Models.FaceBook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupMessage")]
    public partial class GroupMessage
    {
        public int group_id { get; set; }

        public int user_id { get; set; }

        [StringLength(200)]
        public string content_msg { get; set; }

        public bool? deleted { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        public TimeSpan? time { get; set; }

        [Key]
        public int message_id { get; set; }

        public bool? read { get; set; }

        public virtual Group_Members Group_Members { get; set; }
    }
}
