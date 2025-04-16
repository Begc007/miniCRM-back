using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniCRM_back.Migrations
{
    /// <inheritdoc />
    public partial class FileColumnAddedToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "Comments",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Comments");
        }
    }
}
