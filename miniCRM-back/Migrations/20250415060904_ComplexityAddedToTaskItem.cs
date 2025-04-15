﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniCRM_back.Migrations
{
    /// <inheritdoc />
    public partial class ComplexityAddedToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Complexity",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complexity",
                table: "TaskItems");
        }
    }
}
