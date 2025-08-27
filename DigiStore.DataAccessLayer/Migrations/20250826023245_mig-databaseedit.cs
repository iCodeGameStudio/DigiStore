using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigiStore.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migdatabaseedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "مدیر" },
                    { 2, "فروشگاه" },
                    { 3, "کاربر" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "MailAddress", "MailPassword", "SiteDescription", "SiteKeys", "SiteName", "SmsApi", "SmsSender" },
                values: new object[] { 1, "aLI@GMAIL.COM", "123456789", "دیجی استور بزرگترین فروشگاه اینترنتی", "خرید اینترنتی آنلاین شاپ فروشگاه اینترنتی", "دیجی استور", "565SFSDFAD65SDFGADASD65", "3000900" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
