﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feed.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FeedEntitySoftDeleteAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Feeds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Feeds");
        }
    }
}
