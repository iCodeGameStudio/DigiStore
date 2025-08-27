using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiStore.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migstoreBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BankCard",
                table: "Stores",
                type: "bigint",
                maxLength: 24,
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCard",
                table: "Stores");
        }
    }
}
