namespace Green_Cycle.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Zones_And_Machines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Machines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Address = c.String(),
                        ZoneId = c.Int(nullable: false),
                        Status = c.String(),
                        FillPercent = c.Int(nullable: false),
                        Lat = c.Double(),
                        Lng = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceZones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.ServiceZones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GeoJson = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Machines", "ZoneId", "dbo.ServiceZones");
            DropIndex("dbo.Machines", new[] { "ZoneId" });
            DropTable("dbo.Materials");
            DropTable("dbo.ServiceZones");
            DropTable("dbo.Machines");
        }
    }
}
