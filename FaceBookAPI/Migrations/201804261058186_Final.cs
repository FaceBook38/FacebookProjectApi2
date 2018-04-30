namespace FaceBookAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blocked_Users",
                c => new
                    {
                        user_id = c.Int(nullable: false),
                        user_block_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.user_id, t.user_block_id })
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        user_name = c.String(nullable: false, maxLength: 50),
                        user_email = c.String(nullable: false, maxLength: 50, unicode: false),
                        user_password = c.String(nullable: false, maxLength: 50),
                        deleted = c.Boolean(),
                        user_type = c.String(maxLength: 10, fixedLength: true),
                        fname = c.String(nullable: false, maxLength: 50),
                        lname = c.String(maxLength: 50),
                        bio = c.String(maxLength: 500),
                        age = c.Int(),
                        profile_image = c.Binary(storeType: "image"),
                    })
                .PrimaryKey(t => t.user_id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        comment_id = c.Int(nullable: false, identity: true),
                        post_id = c.Int(nullable: false),
                        user_id = c.Int(nullable: false),
                        content = c.String(maxLength: 100),
                        deleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.comment_id)
                .ForeignKey("dbo.Posts", t => t.post_id)
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.post_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        post_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        content = c.String(maxLength: 500),
                        deleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.post_id)
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.User_likes",
                c => new
                    {
                        like_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(),
                        post_id = c.Int(),
                    })
                .PrimaryKey(t => t.like_id)
                .ForeignKey("dbo.Posts", t => t.post_id)
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.user_id)
                .Index(t => t.post_id);
            
            CreateTable(
                "dbo.Group_Members",
                c => new
                    {
                        group_id = c.Int(nullable: false),
                        user_id = c.Int(nullable: false),
                        join = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.group_id, t.user_id })
                .ForeignKey("dbo.Groups", t => t.group_id)
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.group_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        group_id = c.Int(nullable: false, identity: true),
                        group_admin = c.Int(nullable: false),
                        group_name = c.String(nullable: false, maxLength: 50, unicode: false),
                        group_image = c.Binary(storeType: "image"),
                        deleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.group_id)
                .ForeignKey("dbo.Users", t => t.group_admin)
                .Index(t => t.group_admin);
            
            CreateTable(
                "dbo.Group_Posts",
                c => new
                    {
                        post_id = c.Int(nullable: false),
                        group_id = c.Int(nullable: false),
                        content = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.post_id, t.group_id })
                .ForeignKey("dbo.Groups", t => t.group_id)
                .Index(t => t.group_id);
            
            CreateTable(
                "dbo.GroupMessage",
                c => new
                    {
                        message_id = c.Int(nullable: false, identity: true),
                        group_id = c.Int(nullable: false),
                        user_id = c.Int(nullable: false),
                        content_msg = c.String(maxLength: 200),
                        deleted = c.Boolean(),
                        date = c.DateTime(storeType: "date"),
                        time = c.Time(precision: 7),
                        read = c.Boolean(),
                    })
                .PrimaryKey(t => t.message_id)
                .ForeignKey("dbo.Group_Members", t => new { t.group_id, t.user_id })
                .Index(t => new { t.group_id, t.user_id });
            
            CreateTable(
                "dbo.User_Friends",
                c => new
                    {
                        user_id = c.Int(nullable: false),
                        user_friend_id = c.Int(nullable: false),
                        request = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.user_id, t.user_friend_id })
                .ForeignKey("dbo.Users", t => t.user_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.UsersMessage",
                c => new
                    {
                        message_id = c.Int(nullable: false, identity: true),
                        sender_id = c.Int(nullable: false),
                        reciver_id = c.Int(nullable: false),
                        message_content = c.String(maxLength: 200),
                        date = c.DateTime(storeType: "date"),
                        time = c.Time(precision: 7),
                        read = c.Boolean(),
                        deleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.message_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Friends", "user_id", "dbo.Users");
            DropForeignKey("dbo.Posts", "user_id", "dbo.Users");
            DropForeignKey("dbo.Groups", "group_admin", "dbo.Users");
            DropForeignKey("dbo.Group_Members", "user_id", "dbo.Users");
            DropForeignKey("dbo.GroupMessage", new[] { "group_id", "user_id" }, "dbo.Group_Members");
            DropForeignKey("dbo.Group_Posts", "group_id", "dbo.Groups");
            DropForeignKey("dbo.Group_Members", "group_id", "dbo.Groups");
            DropForeignKey("dbo.Comments", "user_id", "dbo.Users");
            DropForeignKey("dbo.User_likes", "user_id", "dbo.Users");
            DropForeignKey("dbo.User_likes", "post_id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "post_id", "dbo.Posts");
            DropForeignKey("dbo.Blocked_Users", "user_id", "dbo.Users");
            DropIndex("dbo.User_Friends", new[] { "user_id" });
            DropIndex("dbo.GroupMessage", new[] { "group_id", "user_id" });
            DropIndex("dbo.Group_Posts", new[] { "group_id" });
            DropIndex("dbo.Groups", new[] { "group_admin" });
            DropIndex("dbo.Group_Members", new[] { "user_id" });
            DropIndex("dbo.Group_Members", new[] { "group_id" });
            DropIndex("dbo.User_likes", new[] { "post_id" });
            DropIndex("dbo.User_likes", new[] { "user_id" });
            DropIndex("dbo.Posts", new[] { "user_id" });
            DropIndex("dbo.Comments", new[] { "user_id" });
            DropIndex("dbo.Comments", new[] { "post_id" });
            DropIndex("dbo.Blocked_Users", new[] { "user_id" });
            DropTable("dbo.UsersMessage");
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.User_Friends");
            DropTable("dbo.GroupMessage");
            DropTable("dbo.Group_Posts");
            DropTable("dbo.Groups");
            DropTable("dbo.Group_Members");
            DropTable("dbo.User_likes");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
            DropTable("dbo.Blocked_Users");
        }
    }
}
