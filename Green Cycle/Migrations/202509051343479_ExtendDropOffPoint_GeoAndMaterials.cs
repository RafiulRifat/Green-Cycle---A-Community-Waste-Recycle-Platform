namespace Green_Cycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendDropOffPoint_GeoAndMaterials : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DropOffPoints", "Latitude", c => c.Double());
            AddColumn("dbo.DropOffPoints", "Longitude", c => c.Double());
            AddColumn("dbo.DropOffPoints", "MaterialsAccepted", c => c.String(maxLength: 400));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DropOffPoints", "MaterialsAccepted");
            DropColumn("dbo.DropOffPoints", "Longitude");
            DropColumn("dbo.DropOffPoints", "Latitude");
        }
    }
}
