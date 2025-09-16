namespace Green_Cycle.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepositTransactions", "MachineId", "dbo.Machines");
            DropForeignKey("dbo.MachineIncidents", "MachineId", "dbo.Machines");
            DropForeignKey("dbo.MachineMaintenances", "MachineId", "dbo.Machines");
            DropIndex("dbo.DepositTransactions", new[] { "MachineId" });
            DropIndex("dbo.MachineIncidents", new[] { "MachineId" });
            DropIndex("dbo.MachineMaintenances", new[] { "MachineId" });
            DropTable("dbo.DepositTransactions");
            DropTable("dbo.MachineIncidents");
            DropTable("dbo.MachineMaintenances");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MachineMaintenances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastRefilledUtc = c.DateTime(nullable: false),
                        LastResetUtc = c.DateTime(nullable: false),
                        MachineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MachineIncidents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssueCode = c.String(nullable: false, maxLength: 24),
                        Description = c.String(nullable: false, maxLength: 256),
                        Status = c.Int(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false),
                        ResolvedUtc = c.DateTime(),
                        MachineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepositTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserDisplayName = c.String(nullable: false, maxLength: 64),
                        Material = c.String(nullable: false, maxLength: 64),
                        WeightKg = c.Double(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false),
                        MachineId = c.Int(nullable: false),
                        PointsAwarded = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.MachineMaintenances", "MachineId");
            CreateIndex("dbo.MachineIncidents", "MachineId");
            CreateIndex("dbo.DepositTransactions", "MachineId");
            AddForeignKey("dbo.MachineMaintenances", "MachineId", "dbo.Machines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MachineIncidents", "MachineId", "dbo.Machines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DepositTransactions", "MachineId", "dbo.Machines", "Id", cascadeDelete: true);
        }
    }
}
