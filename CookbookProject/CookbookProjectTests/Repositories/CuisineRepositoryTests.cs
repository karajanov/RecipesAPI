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
    public class CuisineRepositoryTests
    {
        private ICuisineRepository cuisineRepository;

        public CuisineRepositoryTests()
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectCuisine()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var cuisineId = 4;

            //Actual
            var actual = await cuisineRepository.GetByIdAsync(cuisineId);

            //Assert
            Assert.Equal(cuisineId, actual.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var cuisineId = 55;

            //Actual
            var actual = await cuisineRepository.GetByIdAsync(cuisineId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAllTitles_ShouldReturnCorrectAmount()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var expectedCount = 5;

            //Actual
            var actual = await cuisineRepository.GetAllTitlesAsync();

            //Assert
            Assert.Equal(expectedCount, actual.Count());
        }

        [Theory]
        [InlineData(1, "Mexican")]
        [InlineData(2, "Italian")]
        public async Task GetIdByTitle_ShouldReturnCorrectId(int expectedId, string title)
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange

            //Actual
            var actual = await cuisineRepository.GetIdByTitleAsync(title);

            //Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public async Task GetIdByTitle_ShouldReturnZero()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var title = "Test";
            var expectedId = 0;

            //Actual
            var actual = await cuisineRepository.GetIdByTitleAsync(title);

            //Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public async Task Insert_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var cuisine = new Cuisine() { Title = "NewCuisine" };
            var expectedCount = 6;

            //Actual
            await cuisineRepository.InsertAsync(cuisine);
            var actual = await cuisineRepository.GetByIdAsync(cuisine.Id);
            var actualCount = await dbContext.Cuisine.CountAsync();
            
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(cuisine.Title, actual.Title);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldWorkForExistentTitle()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var title = "Mexican";
            var expectedId = await cuisineRepository.GetIdByTitleAsync(title);
            var expectedCount = 5;

            //Actual
            var actual = await cuisineRepository.InsertIfNecessaryAsync(title);
            var actualCount = await dbContext.Cuisine.CountAsync();

            //Assert
            Assert.Equal(expectedId, actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldWorkForNonExistentTitle()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            var title = "Mediterranean";
            var expectedCount = 6;

            //Actual
            var actual = await cuisineRepository.InsertIfNecessaryAsync(title);
            var actualCount = await dbContext.Cuisine.CountAsync();
            
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            cuisineRepository = new CuisineRepository(dbContext);

            //Arrange
            string title = null;

            //Actual
            var actual = await cuisineRepository.InsertIfNecessaryAsync(title);

            //Assert
            Assert.Null(actual);
        }
    }
}
