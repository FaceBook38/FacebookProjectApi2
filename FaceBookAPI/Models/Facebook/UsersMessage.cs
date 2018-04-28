namespace FaceBookAPI.Models.FaceBook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsersMessage")]
    public partial class UsersMessage
    {
        public int sender_id { get; set; }

        public int reciver_id { get; set; }

        [StringLength(200)]
        public string message_content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        public TimeSpan? time { get; set; }

        public bool? read { get; set; }

        public bool? deleted { get; set; }

        [Key]
        public int message_id { get; set; }
        public virtual User Sender { get; set; }

        public virtual User Receiver { get; set; }
    }
}
