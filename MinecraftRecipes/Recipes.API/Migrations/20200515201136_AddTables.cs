using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Recipes.API.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    Pattern = table.Column<string>(nullable: true),
                    ResultId = table.Column<int>(nullable: true),
                    ResultCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Items_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatternKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Character = table.Column<string>(nullable: true),
                    ItemId = table.Column<int>(nullable: true),
                    RecipeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatternKeys_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatternKeys_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatternKeys_ItemId",
                table: "PatternKeys",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PatternKeys_RecipeId",
                table: "PatternKeys",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ResultId",
                table: "Recipes",
                column: "ResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatternKeys");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
