using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendBookstore.Migrations
{
    /// <inheritdoc />
    public partial class knkn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Description22",
                table: "category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Description22",
                table: "category",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
