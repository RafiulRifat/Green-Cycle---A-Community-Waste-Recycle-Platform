namespace Green_Cycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecognitionCountToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RecognitionCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RecognitionCount");
        }
    }
}
