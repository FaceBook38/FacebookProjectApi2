namespace FacebookConsumer.Models.FaceBook
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FacebookContext : DbContext
    {
        public FacebookContext()
            : base("name=FacebookContext")
        {
        }

        public virtual DbSet<Blocked_Users> Blocked_Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Group_Members> Group_Members { get; set; }
        public virtual DbSet<Group_Posts> Group_Posts { get; set; }
        public virtual DbSet<GroupMessage> GroupMessages { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        //public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User_Friends> User_Friends { get; set; }
        public virtual DbSet<User_likes> User_likes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersMessage> UsersMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group_Members>()
                .HasMany(e => e.GroupMessages)
                .WithRequired(e => e.Group_Members)
                .HasForeignKey(e => new { e.group_id, e.user_id })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.group_name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Group_Members)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Group_Posts)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.user_type)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Blocked_Users)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Group_Members)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.group_admin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.User_Friends)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
