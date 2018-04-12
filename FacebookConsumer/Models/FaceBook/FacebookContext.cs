using System.Data.Entity;

namespace FacebookConsumer.Models.FaceBook
{
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
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User_Friends> User_Friends { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
                .Property(e => e.bio)
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
