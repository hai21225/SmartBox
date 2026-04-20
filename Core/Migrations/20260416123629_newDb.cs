using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class newDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PriceExpireAt",
                table: "Usages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TempPrice",
                table: "Usages",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceExpireAt",
                table: "Usages");

            migrationBuilder.DropColumn(
                name: "TempPrice",
                table: "Usages");
        }
    }
}
