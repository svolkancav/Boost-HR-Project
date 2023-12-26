using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Project.Infrastructure.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "MasterExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "MasterExpenses");

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
