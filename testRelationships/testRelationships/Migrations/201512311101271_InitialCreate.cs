namespace testRelationships.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Agent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agents", t => t.Agent_Id)
                .Index(t => t.Agent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Agent_Id", "dbo.Agents");
            DropIndex("dbo.Employees", new[] { "Agent_Id" });
            DropTable("dbo.Employees");
            DropTable("dbo.Agents");
        }
    }
}
