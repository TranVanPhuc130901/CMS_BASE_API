using CMS_WT_API.Model;
using Microsoft.EntityFrameworkCore;

namespace CMS_DB
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        #region Db Set
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductContent> ProductContents { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductMetaData> ProductMetaDatas { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        //public DbSet<Account> Users { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }

        //public DbSet<RolePermission> RolePermissions { get; set; }

        //public DbSet<Role> Roles { get; set; }

        //public DbSet<Permission> Permissions { get; set; }

        #endregion

        #region relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(c => c.CategoryID);
                e.Property(c => c.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Product");
                e.HasKey(p => p.ProductID);
                e.Property(p => p.ProductCode).HasMaxLength(20);
                e.Property(p => p.ProductName).HasMaxLength(255);
                e.Property(p => p.ProductDescription).HasMaxLength(255);
                e.Property(p => p.CreatedDate).HasDefaultValueSql("getDate()");
                e.Property(p => p.ModifinedDate).HasDefaultValueSql("getDate()");
            });

            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.ToTable("ProductCategory");
                e.HasKey(pc => new { pc.ProductCategoryID, pc.ProductID, pc.CategoryID });

                e.HasOne(p => p.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(p => p.ProductID)
                    .HasConstraintName("Fk_ProductCategory_Product");

                e.HasOne(c => c.Category)
                    .WithMany(c => c.ProductCategories)
                    .HasForeignKey(c => c.CategoryID)
                    .HasConstraintName("Fk_ProductCategory_Category");
            });

            modelBuilder.Entity<ProductContent>(e =>
            {
                e.ToTable("ProductContent");
                e.HasKey(pc => pc.ProductContentID);
                e.Property(pc => pc.ProductContentName).HasColumnType("varchar(MAX)");

                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductContents)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductContent_Product");
            });

            modelBuilder.Entity<ProductImage>(e =>
            {
                e.ToTable("ProductImage");
                e.HasKey(pi => pi.ProductImageID);
                e.Property(pi => pi.ProductImageSlug).HasColumnType("varchar(MAX)");

                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductImage_Product");
            });
            modelBuilder.Entity<ProductMetaData>(e =>
            {
                e.ToTable("ProductMetaData");
                e.HasKey(pmd => pmd.ProductMetaDataID);
                e.Property(pmd => pmd.ProductMetaDataTitle).HasMaxLength(255);
                e.Property(pmd => pmd.ProductMetadataDescrition).HasColumnType("varchar(MAX)");
                e.Property(pmd => pmd.CreatedDate).HasDefaultValueSql("getDate()");
                e.Property(pmd => pmd.ModifiedDate).HasDefaultValueSql("getDate()");

                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductMetaDatas)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductMetaData_Product");
            });

            modelBuilder.Entity<ProductPrice>(e =>
            {
                e.ToTable("ProductPrice");
                e.HasKey(pp => pp.ProductPriceID);
                e.Property(pp => pp.ProductCost).HasColumnType("Decimal(18,4)");
                e.Property(pp => pp.ProductPromotional).HasColumnType("Decimal(18,4)");

                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductPrices)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductPrice_Product");
            });
            //modelBuilder.Entity<Account>(e =>
            //{
            //    e.ToTable("User");
            //    e.HasKey(a => a.UserID);
            //    e.Property(a => a.Username).HasColumnType("varchar(50)");
            //    e.Property(a => a.Email).HasColumnType("varchar(50)");
            //    e.Property(a => a.Fullname).HasColumnType("varchar(100)");
            //});

            //modelBuilder.Entity<Permission>(e =>
            //{
            //    e.ToTable("Permission");
            //    e.HasKey(p => p.PermissionID);
            //    e.Property(p => p.PermissionsName).HasColumnType("varchar(100)");
            //    e.Property(p => p.PermissionDesc).HasColumnType("varchar(500)");
            //});

            //modelBuilder.Entity<UserRole>(e =>
            //{
            //    e.ToTable("UserRole");
            //    e.HasKey(ap => new { ap.RoleID, ap.UserID });

            //    e.HasOne(a => a.User)
            //    .WithMany(a => a.userRoles)
            //    .HasForeignKey(a => a.UserID)
            //    .HasConstraintName("FK_UserRole_User");

            //    e.HasOne(p => p.Role)
            //    .WithMany(p => p.UserRoles)
            //    .HasForeignKey(p => p.RoleID)
            //    .HasConstraintName("FK_UserRole_Role");
            //});

            //modelBuilder.Entity<Role>(e => {
            //    e.ToTable("Role");
            //    e.HasKey(pg => pg.RoleID);
            //    e.Property(pg => pg.RoleName).HasColumnType("varchar(255)");
            //    e.Property(pg => pg.RoleDescription).HasColumnType("varchar(500)");
            //});

            //modelBuilder.Entity<RolePermission>(e => {
            //    e.ToTable("RolePermission");
            //    e.HasKey(gp => new { gp.RoleID, gp.PermissionID });

            //    e.HasOne(p => p.Permission)
            //   .WithMany(p => p.RolePermission)
            //   .HasForeignKey(p => p.PermissionID)
            //   .HasConstraintName("FK_RolePermission_Permission");

            //    e.HasOne(pg => pg.Role)
            //    .WithMany(pg => pg.RolePermission)
            //    .HasForeignKey(pg => pg.RoleID)
            //    .HasConstraintName("FK_RolePermission_Role");
            //});
        }
        #endregion
    }
}
