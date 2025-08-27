using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiStore.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migBannerDetailsUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "BannerDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "BannerDetails");
        }
    }
}
