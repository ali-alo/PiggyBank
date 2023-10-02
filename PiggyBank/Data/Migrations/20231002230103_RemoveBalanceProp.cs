using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiggyBank.Data.Migrations
{
    public partial class RemoveBalanceProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "AspNetUsers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
