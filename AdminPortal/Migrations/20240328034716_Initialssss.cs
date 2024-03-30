using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin.Portal.API.Migrations
{
    /// <inheritdoc />
    public partial class Initialssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Users",
                newName: "UserType");
        }
    }
}
