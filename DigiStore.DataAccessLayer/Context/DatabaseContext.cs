using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;


namespace DigiStore.DataAccessLayer.Context
{
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "مدیر" },
                new Role { Id = 2, Name = "فروشگاه" },
                new Role { Id = 3, Name = "کاربر" }
            );

            modelBuilder.Entity<Setting>().HasData(
                new Setting
                {
                    Id = 1,
                    SiteName = "دیجی استور",
                    SiteDescription = "دیجی استور بزرگترین فروشگاه اینترنتی",
                    SiteKeys = "خرید اینترنتی آنلاین شاپ فروشگاه اینترنتی",
                    SmsApi = "565SFSDFAD65SDFGADASD65",
                    SmsSender = "3000900",
                    MailAddress = "aLI@GMAIL.COM",
                    MailPassword = "123456789"
                }
            );

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   RoleId = 1,
                   Mobile = "123456",
                   Password = "81-DC-9B-DB-52-D0-4D-C2-00-36-DB-D8-31-3E-D0-55", //password = 1234
                   IsActive = true,
                   Date = "1404/05/10",
                   ActiveCode = "0"
               }
           );

            modelBuilder.Entity<Banner>().HasData(
               new Banner
               {
                   Id = 1,
                   Name = "بنر شماره 1",
                   DefaultImage = "4e3d7be09a824278a4ab72965cc009b5.jpg",
                   Size = "820 * 300",
                   Price = 200000000,
                   Day = 30
               },
               new Banner
               {
                   Id = 2,
                   Name = "بنر شماره 2",
                   DefaultImage = "4e3d7be09a824278a4ab72965cc009b5.jpg",
                   Size = "820 * 300",
                   Price = 200000000,
                   Day = 30
               }
           );
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldCategory> FieldCategories { get; set; }
        public DbSet<ProductField> ProductFields { get; set; }
        public DbSet<ProductSeen> ProductSeens { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<BannerDetails> BannerDetails { get; set; }
    }
}
