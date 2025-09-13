namespace Green_Cycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCollectorRoute : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectorRoutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectorId = c.String(maxLength: 128),
                        ZoneName = c.String(),
                        TotalStops = c.Int(nullable: false),
                        CompletedStops = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.String(),
                        DistanceKm = c.Double(nullable: false),
                        EstimatedFinish = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CollectorId)
                .Index(t => t.CollectorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CollectorRoutes", "CollectorId", "dbo.AspNetUsers");
            DropIndex("dbo.CollectorRoutes", new[] { "CollectorId" });
            DropTable("dbo.CollectorRoutes");
        }
    }
}
