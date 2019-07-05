namespace TaskManager.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewPropertyIsTaskCompleteTotblTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTask", "IsTaskComplete", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTask", "IsTaskComplete");
        }
    }
}
