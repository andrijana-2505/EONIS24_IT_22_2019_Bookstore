using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendBookstore.Migrations
{
    /// <inheritdoc />
    public partial class AddInCategorykk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description22",
                table: "category",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description22",
                table: "category");
        }
    }
}
