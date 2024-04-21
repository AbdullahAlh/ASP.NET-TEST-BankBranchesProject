using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankBranches.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Brances",
                table: "Brances");

            migrationBuilder.RenameTable(
                name: "Brances",
                newName: "Branches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Brances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brances",
                table: "Brances",
                column: "Id");
        }
    }
}
