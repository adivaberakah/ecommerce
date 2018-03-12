namespace eCommerce.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String());
            AddColumn("dbo.Products", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Products", "ProductDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductDescription", c => c.String());
            DropColumn("dbo.Products", "CostPrice");
            DropColumn("dbo.Products", "Description");
        }
    }
}
