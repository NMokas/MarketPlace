using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProductToDbAndRemoveCategoryDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryDto_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "CategoryDto");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryDto_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "CategoryDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
