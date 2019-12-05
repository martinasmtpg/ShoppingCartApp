namespace ShoppingCartApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelTransTransactionItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_m_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Int(nullable: false),
                        OrderDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_t_TransactionItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Item_Id = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_m_item", t => t.Item_Id)
                .ForeignKey("dbo.tb_m_transaction", t => t.Transaction_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Transaction_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_t_TransactionItem", "Transaction_Id", "dbo.tb_m_transaction");
            DropForeignKey("dbo.tb_t_TransactionItem", "Item_Id", "dbo.tb_m_item");
            DropIndex("dbo.tb_t_TransactionItem", new[] { "Transaction_Id" });
            DropIndex("dbo.tb_t_TransactionItem", new[] { "Item_Id" });
            DropTable("dbo.tb_t_TransactionItem");
            DropTable("dbo.tb_m_transaction");
        }
    }
}
