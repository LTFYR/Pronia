using Microsoft.EntityFrameworkCore.Migrations;

namespace Pronia.Migrations
{
    public partial class uptadedSlider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sliders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Sliders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Sliders");
        }
    }
}
