namespace Green_Cycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRouteStops : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RouteStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RouteId = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Address = c.String(maxLength: 256),
                        Order = c.Int(nullable: false),
                        ScheduledTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        Status = c.String(maxLength: 32),
                        CompletedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CollectorRoutes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteStops", "RouteId", "dbo.CollectorRoutes");
            DropIndex("dbo.RouteStops", new[] { "RouteId" });
            DropTable("dbo.RouteStops");
        }
    }
}
