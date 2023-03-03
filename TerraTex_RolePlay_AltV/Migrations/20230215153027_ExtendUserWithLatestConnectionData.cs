using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerraTexRolePlayAltVServer.Migrations
{
    /// <inheritdoc />
    public partial class ExtendUserWithLatestConnectionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "LastHardwareIdExHash",
                table: "Users",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "LastHardwareIdHash",
                table: "Users",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIp",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<ulong>(
                name: "LastSocialClubId",
                table: "Users",
                type: "bigint unsigned",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastHardwareIdExHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastHardwareIdHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastIp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastSocialClubId",
                table: "Users");
        }
    }
}
