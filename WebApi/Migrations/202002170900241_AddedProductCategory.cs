namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProductDetails", "ProductModelName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductDetails", "ProductModelName");
            DropTable("dbo.ProductCategories");
        }
    }
}
