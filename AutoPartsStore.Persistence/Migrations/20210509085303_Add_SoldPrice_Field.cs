using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoPartsStore.Persistence.Migrations
{
    public partial class Add_SoldPrice_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SoldPrice",
                table: "ProductCards",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldPrice",
                table: "ProductCards");
        }
    }
}
