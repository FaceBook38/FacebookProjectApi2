namespace FaceBookAPI.Models.Testing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        [Key]
        public int comment_id { get; set; }

        public int post_id { get; set; }

        public int user_id { get; set; }

        [StringLength(100)]
        public string content { get; set; }

        public bool? deleted { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}
