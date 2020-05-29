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
    public class CategoryRepositoryTests
    {
        private ICategoryRepository categoryRepository;

        public CategoryRepositoryTests()
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectCategory()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var categoryId = 3;

            //Actual
            var actual = await categoryRepository.GetByIdAsync(categoryId);

            //Assert
            Assert.Equal(categoryId, actual.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var categoryId = 33;

            //Actual
            var actual = await categoryRepository.GetByIdAsync(categoryId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAllTitles_ShouldReturnCorrectAmount()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var expectedCount = 5;

            //Actual
            var actual = await categoryRepository.GetAllTitlesAsync();

            //Assert
            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async Task GetIdByTitle_ShouldReturnCorrectId()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var title = "Dessert";
            var expectedId = 3;

            //Actual
            var actual = await categoryRepository.GetIdByTitleAsync(title);

            //Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public async Task GetIdByTitle_ShouldReturnZero()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var title = "Candy";
            var expectedId = 0;

            //Actual
            var actual = await categoryRepository.GetIdByTitleAsync(title);

            //Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public async Task Insert_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var category = new Category() { Title = "NewTitle" };
            var expectedCount = 6;

            //Actual
            await categoryRepository.InsertAsync(category);
            var actual = await categoryRepository.GetByIdAsync(category.Id);
            var categories = await dbContext.Categories.CountAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, categories);
            Assert.Equal(category.Title, actual.Title);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldWorkForExistentTitle()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var title = "Fast Food";
            var expectedId = 2;
            var expectedCount = 5;

            //Actual
            var actual = await categoryRepository.InsertIfNecessaryAsync(title);
            var categoryCount = await dbContext.Categories.CountAsync();

            //Assert
            Assert.Equal(expectedId, actual);
            Assert.Equal(expectedCount, categoryCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldWorkForNonExistentTitle()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            var title = "New Category";
            var expectedCount = 6;

            //Actual
            var actual = await categoryRepository.InsertIfNecessaryAsync(title);
            var categoryCount = await dbContext.Categories.CountAsync();
            
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, categoryCount);
        }

        [Fact]
        public async Task InsertIfNecessary_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            categoryRepository = new CategoryRepository(dbContext);

            //Arrange
            string title = null;

            //Actual
            var actual = await categoryRepository.InsertIfNecessaryAsync(title);

            //Assert
            Assert.Null(actual);
        }
    }
}
