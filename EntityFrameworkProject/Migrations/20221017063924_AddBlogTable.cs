using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkProject.Migrations
{
    public partial class AddBlogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Date", "Desc", "Image", "IsDeleted", "Title" },
                values: new object[] { 1, new DateTime(2022, 10, 17, 10, 39, 23, 468, DateTimeKind.Local).AddTicks(5272), "Desc1", "blog-feature-img-1.jpg", false, "Blog1" });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Date", "Desc", "Image", "IsDeleted", "Title" },
                values: new object[] { 2, new DateTime(2022, 10, 17, 10, 39, 23, 475, DateTimeKind.Local).AddTicks(648), "Desc2", "blog-feature-img-3.jpg", false, "Blog2" });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Date", "Desc", "Image", "IsDeleted", "Title" },
                values: new object[] { 3, new DateTime(2022, 10, 17, 10, 39, 23, 475, DateTimeKind.Local).AddTicks(1009), "Desc3", "blog-feature-img-4.jpg", false, "Blog3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
