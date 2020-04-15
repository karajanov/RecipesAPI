using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CookbookProject.Migrations
{
    public partial class InitialCreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cuisine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(unicode: false, maxLength: 320, nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<byte[]>(type: "binary(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 30, nullable: false),
                    Instructions = table.Column<string>(maxLength: 800, nullable: false),
                    PrepTime = table.Column<string>(maxLength: 15, nullable: true),
                    CuisineId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Recipes_Cuisine_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(nullable: true),
                    IngredientId = table.Column<int>(nullable: true),
                    Quantity = table.Column<string>(maxLength: 20, nullable: true),
                    Consistency = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Pasta" },
                    { 2, "Fast Food" },
                    { 3, "Dessert" },
                    { 4, "Pastry" },
                    { 5, "Stew" }
                });

            migrationBuilder.InsertData(
                table: "Cuisine",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Mexican" },
                    { 2, "Italian" },
                    { 3, "American" },
                    { 4, "British" },
                    { 5, "Indian" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 11, "Tortillas" },
                    { 10, "Parmesan" },
                    { 9, "Beef" },
                    { 8, "Tomatoes" },
                    { 7, "Garlic" },
                    { 2, "Cherries" },
                    { 5, "Milk" },
                    { 4, "Sugar" },
                    { 3, "Eggs" },
                    { 12, "Salmon" },
                    { 1, "All-purpose Flour" },
                    { 6, "Spaghetti" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Username" },
                values: new object[] { 1, "karajanovb@yahoo.com", new byte[] { 49, 162, 213, 40, 38, 131, 237, 179, 162, 44, 86, 95, 25, 154, 169, 111, 185, 255, 179, 16, 122, 243, 90, 173, 146, 238, 28, 213, 103, 207, 194, 93 }, "UserOne" });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CuisineId", "Instructions", "PrepTime", "Title", "UserId" },
                values: new object[] { 1, 2, 1, "Instructions for Fish Tacos", "25 min", "Fish Tacos", 1 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CuisineId", "Instructions", "PrepTime", "Title", "UserId" },
                values: new object[] { 2, 3, 3, "Instructions for Cherry Cobbler", "45 min", "Cherry Cobbler", 1 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CuisineId", "Instructions", "PrepTime", "Title", "UserId" },
                values: new object[] { 3, 1, 2, "Instructions for Beef Ragu", "55 min", "Beef Ragu", 1 });

            migrationBuilder.InsertData(
                table: "Measurements",
                columns: new[] { "Id", "Consistency", "IngredientId", "Quantity", "RecipeId" },
                values: new object[,]
                {
                    { 1, null, 11, "3", 1 },
                    { 2, "fillet", 12, "200 gr", 1 },
                    { 3, null, 1, "250 gr", 2 },
                    { 4, "frozen", 2, "100 gr", 2 },
                    { 5, "beaten", 3, "4", 2 },
                    { 6, null, 4, "150 gr", 2 },
                    { 7, "Skimmed", 5, "300 ml", 2 },
                    { 8, null, 6, "100 gr", 3 },
                    { 9, "crushed", 7, "1 clove", 3 },
                    { 10, null, 8, "1 can", 3 },
                    { 11, "minced", 9, "200 gr", 3 },
                    { 12, null, 10, "2 teaspoons", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Title",
                table: "Categories",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuisine_Title",
                table: "Cuisine",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Title",
                table: "Ingredients",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_IngredientId",
                table: "Measurements",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_RecipeId",
                table: "Measurements",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CuisineId",
                table: "Recipes",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Username",
                table: "Users",
                columns: new[] { "Email", "Username" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Cuisine");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
