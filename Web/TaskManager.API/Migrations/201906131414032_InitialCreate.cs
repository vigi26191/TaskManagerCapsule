namespace TaskManager.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTask",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentTaskId = c.Int(),
                        TaskName = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblTask", t => t.ParentTaskId)
                .Index(t => t.ParentTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTask", "ParentTaskId", "dbo.tblTask");
            DropIndex("dbo.tblTask", new[] { "ParentTaskId" });
            DropTable("dbo.tblTask");
        }
    }
}
