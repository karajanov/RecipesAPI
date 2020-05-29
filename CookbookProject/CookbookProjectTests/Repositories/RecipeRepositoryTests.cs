using CookbookProject.Models;
using CookbookProject.Services.Repository;
using CookbookProject.Services.Repository.Interfaces;
using CookbookProjectTests.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CookbookProjectTests.Repositories
{
    public class RecipeRepositoryTests
    {
        private IRecipeRepository recipeRepository;

        public RecipeRepositoryTests()
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectRecipe()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 1;

            //Actual
            var actual = await recipeRepository.GetByIdAsync(recipeId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(recipeId, actual.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 99;

            //Actual
            var actual = await recipeRepository.GetByIdAsync(recipeId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetPreviewByCategory_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var expectedCount = 1;
            var category = "Pasta";
            var title = "Beef Ragu";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewByCategoryAsync(category);

            //Assert
            Assert.Equal(expectedCount, actual.Count());
            Assert.Equal(title, actual.First().Title);
        }

        [Fact]
        public async Task GetPreviewByCuisine_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var expectedCount = 1;
            var cuisine = "Mexican";
            var recipeTitle = "Fish Tacos";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewByCuisineAsync(cuisine);

            //Assert
            Assert.Equal(expectedCount, actual.Count());
            Assert.Equal(recipeTitle, actual.First().Title);
        }

        [Fact]
        public async Task GetPreviewByExactTitle_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeTitle = "Cherry Cobbler";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewByExactTitleAsync(recipeTitle);

            //Assert
            Assert.Equal(recipeTitle, actual.First().Title);
        }

        [Fact]
        public async Task GetPreviewByExactTitle_ShouldReturnEmptyList()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeTitle = "Some Recipe";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewByExactTitleAsync(recipeTitle);

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetPreviewThatContainsKey_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var key = "Tacos";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewThatContainsKeyAsync(key);

            //Assert
            Assert.Contains(key, actual.First().Title);
        }

        [Fact]
        public async Task GetPreviewThatContainsKey_ShouldReturnEmptyList()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var key = "Vegetables";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewThatContainsKeyAsync(key);

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetPreviewThatStartsWithKey_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var key = "Cherry";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewThatStartsWithKeyAsync(key);

            //Assert
            Assert.StartsWith(key, actual.First().Title);
        }

        [Fact]
        public async Task GetPreviewThatStartsWithKey_ShouldReturnEmptyList()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var key = "Fruit";

            //Actual
            var actual = await recipeRepository.GetRecipePreviewThatStartsWithKeyAsync(key);

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetPreviewByAuthor_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var expectedCount = 3;
            var expectedAuthor = "UserOne";

            //Actual
            var actualList = await recipeRepository.GetRecipePreviewByAuthorAsync(expectedAuthor);

            //Assert
            Assert.Equal(expectedCount, actualList.Count());
            Assert.All(actualList, item => Assert.Equal(expectedAuthor, item.Author)); 
        }

        [Fact]
        public async Task GetPreviewByAuthor_ShouldReturnEmptyList()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var expectedAuthor = "SecondUser";

            //Actual
            var actualList = await recipeRepository.GetRecipePreviewByAuthorAsync(expectedAuthor);

            //Assert
            Assert.Empty(actualList);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetFullInfoById_ShouldReturnCorrectResult(int recipeId)
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            //var recipeId = 1;
            var expected = await recipeRepository
                .GetByIdAsync(recipeId);
            var expectedCategory = await dbContext.Categories
                .Where(ct => ct.Id == expected.CategoryId)
                .Select(ct => ct.Title)
                .FirstOrDefaultAsync();
            var expectedCuisine = await dbContext.Cuisine
                .Where(cu => cu.Id == expected.CuisineId)
                .Select(cu => cu.Title)
                .FirstOrDefaultAsync();

            //Actual
            var actual = await recipeRepository.GetFullRecipeInfoByIdAsync(recipeId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.PrepTime, actual.PrepTime);
            Assert.Equal(expected.Instructions, actual.Instructions);
            Assert.Equal(expectedCategory, actual.CategoryTitle);
            Assert.Equal(expectedCuisine, actual.CuisineTitle);
        }

        [Fact]
        public async Task GetFullInfoById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 10;

            //Actual
            var actual = await recipeRepository.GetFullRecipeInfoByIdAsync(recipeId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetInstructionsById_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 2;
            var expected = await recipeRepository.GetByIdAsync(recipeId);

            //Actual
            var actual = await recipeRepository.GetInstructionsByIdAsync(recipeId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Instructions, actual.First());
        }

        [Fact]
        public async Task GetInstructionsById_ShouldReturnEmpty()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 22;
           
            //Actual
            var actual = await recipeRepository.GetInstructionsByIdAsync(recipeId);

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task Insert_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var expectedCount = 4;
            var recipe = new Recipe()
            {
                Title = "Test",
                PrepTime = "15 min",
                CategoryId = 1,
                CuisineId = 1,
                Instructions = "Instructions for Test",
                UserId = 1,
                ImagePath = "somepath.jpg"
            };

            //Actual
            await recipeRepository.InsertAsync(recipe);
            var actual = await recipeRepository.GetByIdAsync(recipe.Id);
            var recipeList = await dbContext.Recipes.ToListAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, recipeList.Count);
            Assert.Equal(recipe.Title, actual.Title);
            Assert.Equal(recipe.PrepTime, actual.PrepTime);
            Assert.Equal(recipe.Instructions, actual.Instructions);
            Assert.Equal(recipe.CategoryId, actual.CategoryId);
            Assert.Equal(recipe.CuisineId, actual.CuisineId);
            Assert.Equal(recipe.UserId, actual.UserId);
            Assert.Equal(recipe.ImagePath, actual.ImagePath);
        }

        [Fact]
        public async Task Update_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipe = new Recipe()
            {
                Id = 1,
                Title = "Updated Title",
                PrepTime = "New Time",
                Instructions = "Updated instructions",
                CategoryId = 3,
                CuisineId = 3,
                UserId = 1,
                ImagePath = null
            };

            //Actual
            await recipeRepository.UpdateAsync(recipe);
            var actual = await recipeRepository.GetByIdAsync(recipe.Id);

            //Assert
            Assert.Equal(recipe.Title, actual.Title);
            Assert.Equal(recipe.PrepTime, actual.PrepTime);
            Assert.Equal(recipe.Instructions, actual.Instructions);
            Assert.Equal(recipe.CategoryId, actual.CategoryId);
            Assert.Equal(recipe.CuisineId, actual.CuisineId);
            Assert.Equal(recipe.UserId, actual.UserId);
            Assert.Equal(recipe.ImagePath, actual.ImagePath);
        }

        [Fact]
        public async Task Update_ShouldNotWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipe = new Recipe()
            {
                Id = 99,
                Title = "Updated Title",
                PrepTime = "New Time",
                Instructions = "Updated instructions",
                CategoryId = 3,
                CuisineId = 3,
                UserId = 1,
                ImagePath = null
            };

            //Actual

            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>
                (() => recipeRepository.UpdateAsync(recipe));
        }

        [Fact]
        public async Task Delete_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 3;
            var expectedCount = await dbContext.Recipes.CountAsync() - 1;

            //Actual
            await recipeRepository.DeleteAsync(recipeId);
            var actual = await recipeRepository.GetByIdAsync(recipeId);
            var actualCount = await dbContext.Recipes.CountAsync();

            //Assert
            Assert.Null(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task Delete_ShouldNotWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            recipeRepository = new RecipeRepository(dbContext);

            //Arrange
            var recipeId = 10;

            //Actual

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>
                (() => recipeRepository.DeleteAsync(recipeId));
        }
    }
}
