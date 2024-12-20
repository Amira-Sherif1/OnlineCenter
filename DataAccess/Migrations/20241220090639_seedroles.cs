using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
           INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
             VALUES (NEWID(), 'Admin', 'ADMIN', NEWID())");
            migrationBuilder.Sql(@"
      INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
        VALUES (NEWID(), 'Student', 'STUDENT', NEWID())");
            migrationBuilder.Sql(@"
        INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
        VALUES (NEWID(), 'Assistant', 'ASSISTANT', NEWID())");

            migrationBuilder.Sql(@"
        INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
        VALUES (NEWID(), 'Teacher', 'TEACHER', NEWID())");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM [dbo].[AspNetRoles]
        WHERE [Name] IN ('Admin', 'Student', 'Assistant','Teacher')");


        }
    }
}
