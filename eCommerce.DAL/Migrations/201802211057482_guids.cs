namespace eCommerce.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class guids : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BasketItems", "BasketId", "dbo.Baskets");
            DropIndex("dbo.BasketItems", new[] { "BasketId" });
            DropPrimaryKey("dbo.Baskets");
            AddColumn("dbo.BasketItems", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.BasketItems", "BasketId");
            AddColumn("dbo.BasketItems", "BasketId", c => c.Guid(nullable: false));
            DropColumn("dbo.Baskets", "BasketId");
            AddColumn("dbo.Baskets", "BasketId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Baskets", "BasketId");
            CreateIndex("dbo.BasketItems", "BasketId");
            AddForeignKey("dbo.BasketItems", "BasketId", "dbo.Baskets", "BasketId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketItems", "BasketId", "dbo.Baskets");
            DropIndex("dbo.BasketItems", new[] { "BasketId" });
            DropPrimaryKey("dbo.Baskets");
            AlterColumn("dbo.Baskets", "BasketId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.BasketItems", "BasketId", c => c.Int(nullable: false));
            DropColumn("dbo.BasketItems", "Quantity");
            AddPrimaryKey("dbo.Baskets", "BasketId");
            CreateIndex("dbo.BasketItems", "BasketId");
            AddForeignKey("dbo.BasketItems", "BasketId", "dbo.Baskets", "BasketId", cascadeDelete: true);
        }
    }
}
