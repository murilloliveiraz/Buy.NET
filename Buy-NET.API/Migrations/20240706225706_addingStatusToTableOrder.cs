using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buy_NET.API.Migrations
{
    /// <inheritdoc />
    public partial class addingStatusToTableOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "orders",
                type: "VARCHAR",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "orders");
        }
    }
}
