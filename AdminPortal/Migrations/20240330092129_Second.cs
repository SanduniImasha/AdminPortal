using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin.Portal.API.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "ID");

            migrationBuilder.AddColumn<int>(
                name: "TenantID",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Roles",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Tenants",
                type: "TEXT",
                nullable: true);
        }
    }
}
