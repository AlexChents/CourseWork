namespace CourseChentsov.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserLogins : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLogins", "ConfirmedEmail", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String());
            DropColumn("dbo.UserLogins", "ConfirmedEmail");
        }
    }
}
