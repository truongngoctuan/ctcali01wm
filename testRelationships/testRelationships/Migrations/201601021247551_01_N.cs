namespace testRelationships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_N : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "AgentID", "dbo.Agents");
            DropIndex("dbo.Employees", new[] { "AgentID" });
            AlterColumn("dbo.Employees", "AgentID", c => c.Int());
            CreateIndex("dbo.Employees", "AgentID");
            AddForeignKey("dbo.Employees", "AgentID", "dbo.Agents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "AgentID", "dbo.Agents");
            DropIndex("dbo.Employees", new[] { "AgentID" });
            AlterColumn("dbo.Employees", "AgentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "AgentID");
            AddForeignKey("dbo.Employees", "AgentID", "dbo.Agents", "Id", cascadeDelete: true);
        }
    }
}
