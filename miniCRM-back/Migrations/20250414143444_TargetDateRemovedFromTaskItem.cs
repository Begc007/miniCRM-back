using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniCRM_back.Migrations
{
    /// <inheritdoc />
    public partial class TargetDateRemovedFromTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetDate",
                table: "TaskItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TargetDate",
                table: "TaskItems",
                type: "datetime2",
                nullable: true);
        }
    }
}
