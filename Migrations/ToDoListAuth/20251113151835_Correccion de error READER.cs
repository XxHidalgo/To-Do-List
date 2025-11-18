using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations.ToDoListAuth
{
    /// <inheritdoc />
    public partial class CorrecciondeerrorREADER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84f67d14-7788-4819-bbde-dd9cd0379c26",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Reader", "READER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84f67d14-7788-4819-bbde-dd9cd0379c26",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Redader", "REDADER" });
        }
    }
}
