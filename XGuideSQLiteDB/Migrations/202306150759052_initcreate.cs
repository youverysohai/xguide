namespace XGuideSQLiteDB
{
    using System.Data.Entity.Migrations;

    public partial class initcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calibrations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 2147483647),
                    ManipulatorId = c.Int(),
                    Orientation = c.Int(),
                    Speed = c.Double(),
                    Acceleration = c.Double(),
                    MotionDelay = c.Int(),
                    XOffset = c.Int(),
                    YOffset = c.Int(),
                    MMPerPixel = c.Double(nullable: false),
                    CRZOffset = c.Double(nullable: false),
                    CYOffset = c.Double(nullable: false),
                    CXOffset = c.Double(nullable: false),
                    Procedure = c.String(maxLength: 2147483647),
                    Mode = c.Boolean(nullable: false),
                    JointRotationAngle = c.Double(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Manipulators", t => t.ManipulatorId)
                .Index(t => t.ManipulatorId, name: "IX_Calibration_ManipulatorId");

            CreateTable(
                "dbo.Manipulators",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Type = c.Int(nullable: false),
                    Name = c.String(maxLength: 2147483647),
                    Description = c.String(maxLength: 2147483647),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Username = c.String(maxLength: 2147483647),
                    Email = c.String(maxLength: 2147483647),
                    PasswordHash = c.String(maxLength: 2147483647),
                    IsActive = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Role = c.Int(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Calibrations", "ManipulatorId", "dbo.Manipulators");
            DropIndex("dbo.Calibrations", "IX_Calibration_ManipulatorId");
            DropTable("dbo.Users");
            DropTable("dbo.Manipulators");
            DropTable("dbo.Calibrations");
        }
    }
}