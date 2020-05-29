using CookbookProject.Models;
using CookbookProject.Services.Repository;
using CookbookProject.Services.Repository.Interfaces;
using CookbookProjectTests.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CookbookProjectTests.Repositories
{
    public class IngredientRepositoryTests
    {
        private IIngredientRepository ingredientRepository;

        public IngredientRepositoryTests()
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectIngredient()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var ingredientId = 1;

            //Actual
            var actual = await ingredientRepository.GetByIdAsync(ingredientId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(ingredientId, actual.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var ingredientId = 24;

            //Actual
            var actual = await ingredientRepository.GetByIdAsync(ingredientId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAllTitles_ShouldReturnCorrectAmount()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var expectedCount = await dbContext.Ingredients.CountAsync();

            //Actual
            var actual = await ingredientRepository.GetAllTitlesAsync();

            //Assert
            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task GetIdByTitle_ShouldReturnCorrectId()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var title = "Eggs";
            var expectedId = 3;

            //Actual
            var actual = await ingredientRepository.GetIdByTitleAsync(title);

            //Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public async Task GetIdByTitle_ShouldReturnZero()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var title = "Peanut Butter";
            var expectedId = 0;

            //Actual
            var actual = await ingredientRepository.GetIdByTitleAsync(title);

            //Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public async Task Insert_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var ingredient = new Ingredient() { Title = "Potatoes" };
            var expectedCount = await dbContext.Ingredients.CountAsync() + 1;

            //Actual
            await ingredientRepository.InsertAsync(ingredient);
            var actual = await ingredientRepository.GetByIdAsync(ingredient.Id);
            var actualCount = await dbContext.Ingredients.CountAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldWorkForExistentTitle()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var title = "Beef";
            var expectedId = await ingredientRepository.GetIdByTitleAsync(title);
            var expectedCount = await dbContext.Ingredients.CountAsync();

            //Actual
            var actual = await ingredientRepository.InsertIfNecessaryAsync(title);
            var actualCount = await dbContext.Ingredients.CountAsync();

            //Assert
            Assert.Equal(expectedId, actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldWorkForNonExistentTitle()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            var title = "Ice Cream";
            var expectedCount = await dbContext.Ingredients.CountAsync() + 1;

            //Actual
            var actual = await ingredientRepository.InsertIfNecessaryAsync(title);
            var actualCount = await dbContext.Ingredients.CountAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            ingredientRepository = new IngredientRepository(dbContext);

            //Arrange
            string title = null;

            //Actual
            var actual = await ingredientRepository.InsertIfNecessaryAsync(title);

            //Assert
            Assert.Null(actual);
        }
    }
}
