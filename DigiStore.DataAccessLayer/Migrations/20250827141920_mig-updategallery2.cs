using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigiStore.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migupdategallery2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "Day", "DefaultImage", "Description", "Name", "Price", "Size" },
                values: new object[,]
                {
                    { 1, 30, "4e3d7be09a824278a4ab72965cc009b5.jpg", null, "بنر شماره 1", 200000000, "820 * 300" },
                    { 2, 30, "4e3d7be09a824278a4ab72965cc009b5.jpg", null, "بنر شماره 2", 200000000, "820 * 300" }
                });

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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActiveCode", "Code", "Date", "FullName", "IsActive", "Mobile", "Password", "RoleId" },
                values: new object[] { 1, "0", null, "1404/05/10", null, true, "123456", "81-DC-9B-DB-52-D0-4D-C2-00-36-DB-D8-31-3E-D0-55", 1 });
        }
    }
}
