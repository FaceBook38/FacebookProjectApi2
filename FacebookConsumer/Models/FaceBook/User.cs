

namespace FacebookConsumer.Models.FaceBook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Blocked_Users = new HashSet<Blocked_Users>();
            Comments = new HashSet<Comment>();
            Group_Members = new HashSet<Group_Members>();
            Groups = new HashSet<Group>();
            Posts = new HashSet<Post>();
            User_Friends = new HashSet<User_Friends>();
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "User name")]
        public string user_name { get; set; }

        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        public string user_email { get; set; }

        [Required]
        [StringLength(50)]
        
        [Display(Name = "Password")]
        public string user_password { get; set; }
        [Display(Name = "Confirm Password")]
        [Compare("user_password", ErrorMessage = "Please match your password")]
        public string confirm_password { get; set; }

        public bool? deleted { get; set; }

        [StringLength(10)]
        public string user_type { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string fname { get; set; }
        [Display(Name = "Last name")]
        [StringLength(50)]
        public string lname { get; set; }

        [StringLength(500)]
        public string bio { get; set; }
        [Display(Name = "Age")]
        public int? age { get; set; }
        [Display(Name = "Profile picture")]
        [Column(TypeName = "image")]
        public byte[] profile_image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blocked_Users> Blocked_Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group_Members> Group_Members { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Friends> User_Friends { get; set; }
    }
}
