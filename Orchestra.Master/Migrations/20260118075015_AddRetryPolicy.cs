using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orchestra.Master.Migrations
{
    /// <inheritdoc />
    public partial class AddRetryPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "maxRetries",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxRetries",
                table: "Jobs");
        }
    }
}
