using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FaceBookAPI.Models.FaceBook
{
    public partial class Like
    {
        [Key]
        public int like_id { get; set; }

        public int post_id { get; set; }

        public int user_id { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}