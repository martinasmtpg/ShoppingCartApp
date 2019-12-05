namespace ShoppingCartApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelUserAuth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_t_user", "Name", c => c.String());
            DropColumn("dbo.tb_m_auth", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_m_auth", "Name", c => c.String());
            DropColumn("dbo.tb_t_user", "Name");
        }
    }
}
