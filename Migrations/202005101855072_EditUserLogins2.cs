namespace CourseChentsov.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserLogins2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLogins", "Token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLogins", "Token");
        }
    }
}
