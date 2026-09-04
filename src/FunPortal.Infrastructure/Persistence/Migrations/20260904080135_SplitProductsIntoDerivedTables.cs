using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunPortal.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SplitProductsIntoDerivedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DurationMonths",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ISBN = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Books_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembershipType = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMonths = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipProducts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_MembershipProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Director = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Videos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "MembershipProducts");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Products",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Products",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMonths",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Products",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MembershipType",
                table: "Products",
                type: "INTEGER",
                nullable: true);
        }
    }
}
