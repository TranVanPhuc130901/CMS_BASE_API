using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS_BASE_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artticle",
                columns: table => new
                {
                    ArticleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(700)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artticle", x => x.ArticleID);
                });
            migrationBuilder.CreateTable(
                name: "CategoryArticle",
                columns: table => new
                {
                    CategoryActicleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryArticle", x => x.CategoryActicleID);
                });
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagID);
                });
            migrationBuilder.CreateTable(
                name: "ArticleCategoryArticle",
                columns: table => new
                {
                    ArticleID = table.Column<int>(type: "int", nullable: false),
                    CategoryArticleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategoryArticle", x => new { x.ArticleID, x.CategoryArticleID });
                    table.ForeignKey(
                        name: "Fk_ArticleCategoryArticle_CategoryArticle",
                        column: x => x.CategoryArticleID,
                        principalTable: "CategoryArticle",
                        principalColumn: "CategoryActicleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Fk_ArtilceCategoryArticle_Article",
                        column: x => x.ArticleID,
                        principalTable: "Artticle",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "TagArticle",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagArticle", x => new { x.ArticleID, x.TagID });
                    table.ForeignKey(
                        name: "FK_TagArticle_Tag",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Fk_TagArticle_Article",
                        column: x => x.ArticleID,
                        principalTable: "Artticle",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => new { x.ArticleID, x.UserID, x.CommentID });
                    table.ForeignKey(
                        name: "FK_Comment_Article",
                        column: x => x.ArticleID,
                        principalTable: "Artticle",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategoryArticle_CategoryArticleID",
                table: "ArticleCategoryArticle",
                column: "CategoryArticleID");
            migrationBuilder.CreateIndex(
                name: "IX_TagArticle_TagID",
                table: "TagArticle",
                column: "TagID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleCategoryArticle");

            migrationBuilder.DropTable(
                name: "Comment");
            migrationBuilder.DropTable(
                name: "TagArticle");
            migrationBuilder.DropTable(
                name: "CategoryArticle");
            migrationBuilder.DropTable(
                name: "Tag");
            migrationBuilder.DropTable(
                name: "Artticle");
        }
    }
}
