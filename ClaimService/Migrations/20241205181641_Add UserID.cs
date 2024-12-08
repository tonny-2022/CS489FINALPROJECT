using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchId",
                table: "ItemClaims",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "MatchId",
                table: "ItemClaims",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "ItemClaims");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ItemClaims",
                newName: "ItemMatchId");
        }
    }
}
