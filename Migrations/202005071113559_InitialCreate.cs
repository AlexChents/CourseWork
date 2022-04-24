namespace CourseChentsov.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasicPriceDaysDeliveries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BasicPrice = c.Double(nullable: false),
                        PriceForKg = c.Double(nullable: false),
                        CountDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DistanceDeliveries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstRegionId = c.Int(nullable: false),
                        SecondRegionId = c.Int(nullable: false),
                        BasicPriceDaysDeliveryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BasicPriceDaysDeliveries", t => t.BasicPriceDaysDeliveryId, cascadeDelete: true)
                .ForeignKey("dbo.Regions", t => t.FirstRegionId)
                .ForeignKey("dbo.Regions", t => t.SecondRegionId, cascadeDelete: true)
                .Index(t => t.FirstRegionId)
                .Index(t => t.SecondRegionId)
                .Index(t => t.BasicPriceDaysDeliveryId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Adress = c.String(nullable: false),
                        MaxWeight = c.Int(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                        StatusDepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .ForeignKey("dbo.StatusDepartments", t => t.StatusDepartmentId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.ScheduleId)
                .Index(t => t.StatusDepartmentId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberDelivery = c.Long(nullable: false),
                        DepartmentSendId = c.Int(nullable: false),
                        DepartmentRecipientId = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        CountSeats = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        DateSend = c.DateTime(nullable: false),
                        DateDelivery = c.DateTime(nullable: false),
                        DeliveryCost = c.Double(nullable: false),
                        Comment = c.String(),
                        UserId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        DispatchRegisterId = c.Int(),
                        StatusPackageId = c.Int(),
                        InRegister = c.Boolean(nullable: false),
                        IsPrint = c.Boolean(nullable: false),
                        Department_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentRecipientId)
                .ForeignKey("dbo.Departments", t => t.DepartmentSendId)
                .ForeignKey("dbo.DispatchRegisters", t => t.DispatchRegisterId)
                .ForeignKey("dbo.Recipients", t => t.RecipientId, cascadeDelete: true)
                .ForeignKey("dbo.StatusPackages", t => t.StatusPackageId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .Index(t => t.DepartmentSendId)
                .Index(t => t.DepartmentRecipientId)
                .Index(t => t.UserId)
                .Index(t => t.RecipientId)
                .Index(t => t.DispatchRegisterId)
                .Index(t => t.StatusPackageId)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.DispatchRegisters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsPrint = c.Boolean(nullable: false),
                        InDepartmentSend = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SenderName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        ThirdName = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        DepartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.UserLogins", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        PasswordHash = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecipientName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        ThirdName = c.String(),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusPackages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WeekdaysTimeWork = c.String(nullable: false),
                        WeekdaysWork = c.String(nullable: false),
                        SaturdayTimeWork = c.String(nullable: false),
                        SaturdayWork = c.String(nullable: false),
                        SundayTimeWork = c.String(nullable: false),
                        SundayWork = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecipientDepartments",
                c => new
                    {
                        Recipient_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipient_Id, t.Department_Id })
                .ForeignKey("dbo.Recipients", t => t.Recipient_Id, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistanceDeliveries", "SecondRegionId", "dbo.Regions");
            DropForeignKey("dbo.DistanceDeliveries", "FirstRegionId", "dbo.Regions");
            DropForeignKey("dbo.Cities", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.Departments", "StatusDepartmentId", "dbo.StatusDepartments");
            DropForeignKey("dbo.Departments", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Packages", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Packages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Packages", "StatusPackageId", "dbo.StatusPackages");
            DropForeignKey("dbo.Packages", "RecipientId", "dbo.Recipients");
            DropForeignKey("dbo.RecipientDepartments", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.RecipientDepartments", "Recipient_Id", "dbo.Recipients");
            DropForeignKey("dbo.DispatchRegisters", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Id", "dbo.UserLogins");
            DropForeignKey("dbo.UserLogins", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Packages", "DispatchRegisterId", "dbo.DispatchRegisters");
            DropForeignKey("dbo.Packages", "DepartmentSendId", "dbo.Departments");
            DropForeignKey("dbo.Packages", "DepartmentRecipientId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "CityId", "dbo.Cities");
            DropForeignKey("dbo.DistanceDeliveries", "BasicPriceDaysDeliveryId", "dbo.BasicPriceDaysDeliveries");
            DropIndex("dbo.RecipientDepartments", new[] { "Department_Id" });
            DropIndex("dbo.RecipientDepartments", new[] { "Recipient_Id" });
            DropIndex("dbo.UserLogins", new[] { "RoleId" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "Id" });
            DropIndex("dbo.DispatchRegisters", new[] { "UserId" });
            DropIndex("dbo.Packages", new[] { "Department_Id" });
            DropIndex("dbo.Packages", new[] { "StatusPackageId" });
            DropIndex("dbo.Packages", new[] { "DispatchRegisterId" });
            DropIndex("dbo.Packages", new[] { "RecipientId" });
            DropIndex("dbo.Packages", new[] { "UserId" });
            DropIndex("dbo.Packages", new[] { "DepartmentRecipientId" });
            DropIndex("dbo.Packages", new[] { "DepartmentSendId" });
            DropIndex("dbo.Departments", new[] { "StatusDepartmentId" });
            DropIndex("dbo.Departments", new[] { "ScheduleId" });
            DropIndex("dbo.Departments", new[] { "CityId" });
            DropIndex("dbo.Cities", new[] { "RegionId" });
            DropIndex("dbo.DistanceDeliveries", new[] { "BasicPriceDaysDeliveryId" });
            DropIndex("dbo.DistanceDeliveries", new[] { "SecondRegionId" });
            DropIndex("dbo.DistanceDeliveries", new[] { "FirstRegionId" });
            DropTable("dbo.RecipientDepartments");
            DropTable("dbo.News");
            DropTable("dbo.StatusDepartments");
            DropTable("dbo.Schedules");
            DropTable("dbo.StatusPackages");
            DropTable("dbo.Recipients");
            DropTable("dbo.Roles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Users");
            DropTable("dbo.DispatchRegisters");
            DropTable("dbo.Packages");
            DropTable("dbo.Departments");
            DropTable("dbo.Cities");
            DropTable("dbo.Regions");
            DropTable("dbo.DistanceDeliveries");
            DropTable("dbo.BasicPriceDaysDeliveries");
        }
    }
}
