namespace Green_Cycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddProfileFields : DbMigration
    {
        public override void Up()
        {
            // FullName with max length (Identity typically uses 256)
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String(maxLength: 256));

            // JoinedOn: not nullable, default to current UTC for existing rows
            AddColumn("dbo.AspNetUsers", "JoinedOn",
                c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "JoinedOn");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}
