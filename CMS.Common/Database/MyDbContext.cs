using Microsoft.EntityFrameworkCore;

namespace CMS_Common.Database
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
        public DbSet<Account> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TagArticle> TagsArticles { get; set; }

        public DbSet<Article> Articels { get; set; }

        public DbSet<CategoryArticle> CategoryArticles { get; set; }

        public DbSet<ArticleCategoryArticle> ArticleCategorieArticles { get; set; }

        public DbSet<Comment> Comments { get; set; }

        #endregion

        #region relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tạo bảng Category(danh mục sản phẩm) trong Db
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(c => c.CategoryID);
                e.Property(c => c.CategoryName).HasMaxLength(255);
            });
            // Tạo bảng Product(sản phẩm) trong db
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
            // tạo bảng ProductCategory(Danh mục và sản phẩm) trong db
            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.ToTable("ProductCategory");
                e.HasKey(pc => new { pc.ProductCategoryID, pc.ProductID, pc.CategoryID });
                // Tạo khóa giữa Product và ProductCategory
                e.HasOne(p => p.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(p => p.ProductID)
                    .HasConstraintName("Fk_ProductCategory_Product");
                // Tạo khóa giữa Category và ProductCategory
                e.HasOne(c => c.Category)
                    .WithMany(c => c.ProductCategories)
                    .HasForeignKey(c => c.CategoryID)
                    .HasConstraintName("Fk_ProductCategory_Category");
            });
            // Tạo bảng ProductContent(Mô tả sản phẩm) trong db
            modelBuilder.Entity<ProductContent>(e =>
            {
                e.ToTable("ProductContent");
                e.HasKey(pc => pc.ProductContentID);
                e.Property(pc => pc.ProductContentName).HasColumnType("varchar(MAX)");
                // Tạo khóa giữa Product và ProductContent
                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductContents)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductContent_Product");
            });
            // Tạo bảng productImage(Ảnh sản phẩm) trong db
            modelBuilder.Entity<ProductImage>(e =>
            {
                e.ToTable("ProductImage");
                e.HasKey(pi => pi.ProductImageID);
                e.Property(pi => pi.ProductImageSlug).HasColumnType("varchar(MAX)");
                // Tạo khóa giữa product và productImage
                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductImage_Product");
            });
            // Tạo bảng ProductMetaData(SEO sản phẩm) trong db
            modelBuilder.Entity<ProductMetaData>(e =>
            {
                e.ToTable("ProductMetaData");
                e.HasKey(pmd => pmd.ProductMetaDataID);
                e.Property(pmd => pmd.ProductMetaDataTitle).HasMaxLength(255);
                e.Property(pmd => pmd.ProductMetadataDescrition).HasColumnType("varchar(MAX)");
                e.Property(pmd => pmd.CreatedDate).HasDefaultValueSql("getDate()");
                e.Property(pmd => pmd.ModifiedDate).HasDefaultValueSql("getDate()");
                // Tạo khóa giữa Product và ProductMetaData
                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductMetaDatas)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductMetaData_Product");
            });
            // Tạo bảng ProuductPrice(giá sản phẩm) trong db
            modelBuilder.Entity<ProductPrice>(e =>
            {
                e.ToTable("ProductPrice");
                e.HasKey(pp => pp.ProductPriceID);
                e.Property(pp => pp.ProductCost).HasColumnType("Decimal(18,4)");
                e.Property(pp => pp.ProductPromotional).HasColumnType("Decimal(18,4)");
                // Tạo khóa giữa Product và ProductPrice
                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductPrices)
                .HasForeignKey(p => p.ProductID)
                .HasConstraintName("Fk_ProductPrice_Product");
            });
            //Tạo bảng Account(người dùng) trong db
            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("User");
                e.HasKey(a => a.UserID);
                e.Property(a => a.Username).HasColumnType("varchar(50)");
                e.Property(a => a.Email).HasColumnType("varchar(50)");
                e.Property(a => a.Fullname).HasColumnType("varchar(100)");
            });
            // Tạo bảng Permission( các quyền trong db)
            modelBuilder.Entity<Permission>(e =>
            {
                e.ToTable("Permission");
                e.HasKey(p => p.PermissionID);
                e.Property(p => p.PermissionsName).HasColumnType("varchar(100)");
                e.Property(p => p.PermissionDesc).HasColumnType("varchar(500)");
            });
            // Tạo bảng UserRole(quyền của người dùng) trong db
            modelBuilder.Entity<UserRole>(e =>
            {
                e.ToTable("UserRole");
                e.HasKey(ap => new { ap.RoleID, ap.UserID });
                // Tạo khóa giữa bảng Account và roleAccount
                e.HasOne(a => a.User)
                .WithMany(a => a.userRoles)
                .HasForeignKey(a => a.UserID)
                .HasConstraintName("FK_UserRole_User");
                // Tạo khóa giữa bảng Role và UserRole
                e.HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleID)
                .HasConstraintName("FK_UserRole_Role");
            });
            // Tạo bảng Role(các nhóm quyền) trong DB
            modelBuilder.Entity<Role>(e => {
                e.ToTable("Role");
                e.HasKey(pg => pg.RoleID);
                e.Property(pg => pg.RoleName).HasColumnType("varchar(255)");
                e.Property(pg => pg.RoleDescription).HasColumnType("varchar(500)");
            });
            //Tạo bảng RolePermission(quyền của các nhóm)
            modelBuilder.Entity<RolePermission>(e => {
                e.ToTable("RolePermission");
                e.HasKey(gp => new { gp.RoleID, gp.PermissionID });
                // tạo khóa giữa bảng Permission và RolePermission
                e.HasOne(p => p.Permission)
               .WithMany(p => p.RolePermission)
               .HasForeignKey(p => p.PermissionID)
               .HasConstraintName("FK_RolePermission_Permission");
                // Tạo khóa giữa bảng Role và RolePermission
                e.HasOne(pg => pg.Role)
                .WithMany(pg => pg.RolePermission)
                .HasForeignKey(pg => pg.RoleID)
                .HasConstraintName("FK_RolePermission_Role");
            });
            // Tạo bảng Article(tin tức)
            modelBuilder.Entity<Article>(e =>
            {
                e.ToTable("Artticle");
                e.HasKey(a => a.ArticleID);
                e.Property(a => a.Title).HasColumnType("nvarchar(255)");
                e.Property(a => a.Content).HasColumnType("nvarchar(700)");
                e.Property(a => a.Description).HasColumnType("nvarchar(MAX)");
                e.Property(a => a.Image).HasColumnType("nvarchar(255)");
                e.Property(a => a.Author).HasColumnType("nvarchar(100)");
            });
            // Tạo bảng CategoryArticle(danh mục tin tức)
            modelBuilder.Entity<CategoryArticle>(e => {
                e.ToTable("CategoryArticle");
                e.HasKey(ca => ca.CategoryActicleID);
                e.Property(ca => ca.CategoryName).HasColumnType("nvarchar(500)");
            });
            // Tạo bảng Tag( từ khóa tin tức)
            modelBuilder.Entity<Tag>(e => {
                e.ToTable("Tag");
                e.HasKey(t => t.TagID);
                e.Property(t => t.TagName).HasColumnType("nvarchar(255)");
            });
       
            // Tạo bảng ArticleCategoryArticle( liên kết của Tin tức và danh mục tin tức)
            modelBuilder.Entity<ArticleCategoryArticle>(e => {
                e.ToTable("ArticleCategoryArticle");
                e.HasKey(aca => new { aca.ArticleID, aca.CategoryArticleID });
                // Tạo khóa giữa bảng CategoryArticle và ArticleCategoryArticle
                e.HasOne(ca => ca.CategoryArticles)
                    .WithMany(ca => ca.ArticleCategoryArticles)
                    .HasForeignKey(ca => ca.CategoryArticleID)
                    .HasConstraintName("Fk_ArticleCategoryArticle_CategoryArticle");
                // Tạo khóa giữa bảng Article và ArticleCategoryArrticle
                e.HasOne(a => a.Articles)
                    .WithMany(a => a.ArticleCategoryArticles)
                    .HasForeignKey(a => a.ArticleID)
                    .HasConstraintName("Fk_ArtilceCategoryArticle_Article");
            });
            // Tạo bảng TagArrticel(liên kết giữa từ khóa Tin tức và Tin tức)
            modelBuilder.Entity<TagArticle>(e => {
                e.ToTable("TagArticle");
                e.HasKey(e => new { e.ArticleID, e.TagID});
                // Tạo khóa giữa bảng Article và TagArrticle
                e.HasOne(a => a.Articles)
                    .WithMany(a => a.TagArticles)
                    .HasForeignKey(a => a.ArticleID)
                    .HasConstraintName("Fk_TagArticle_Article");
                // Tạo khóa giữa bảng Article và TagArticle
                e.HasOne(t => t.Tags)
                    .WithMany(t => t.TagArticles)
                    .HasForeignKey(t => t.TagID)
                    .HasConstraintName("FK_TagArticle_Tag");
            });
            // Tạo bảng CommentUserArticle(Bảng liên kết giữa người dùng, bình luận và bài viết)
            modelBuilder.Entity<Comment>(e => {
                e.ToTable("Comment");
                e.HasKey(cua => new { cua.ArticleID, cua.UserID, cua.CommentID });
                e.Property(cua => cua.Content).HasColumnType("nvarchar(MAX)");
                // Tạo khóa giữa Article và bảng liên kết
                e.HasOne(a => a.Articles)
                    .WithMany(a => a.Comments)
                    .HasForeignKey(a => a.ArticleID)
                    .HasConstraintName("FK_Comment_Article");
                // tạo khóa giữa bảng account và bảng liên kết
                e.HasOne(u => u.Accounts)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(u => u.UserID).
                    HasConstraintName("FK_Comment_Account");
            });
        }
        #endregion
    }
}
