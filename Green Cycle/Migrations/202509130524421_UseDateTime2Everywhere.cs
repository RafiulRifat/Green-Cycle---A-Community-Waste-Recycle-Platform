namespace Green_Cycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UseDateTime2Everywhere : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "JoinedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "JoinedOn", c => c.DateTime(nullable: false));
        }
    }
}
