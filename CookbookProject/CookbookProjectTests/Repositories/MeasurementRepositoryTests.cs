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
    public class MeasurementRepositoryTests
    {
        private IMeasurementRepository measurementRepository;

        public MeasurementRepositoryTests()
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectMeasurement()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurementId = 1;

            //Actual
            var actual = await measurementRepository.GetByIdAsync(measurementId);

            //Assert
            Assert.Equal(measurementId, actual.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurementId = 95;

            //Actual
            var actual = await measurementRepository.GetByIdAsync(measurementId);

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 5)]
        [InlineData(3, 5)]
        public async Task GetMeasurementsByRecipeId_ShouldReturnCorrectAmount(int recipeId, int expectedAmount)
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange

            //Actual
            var actual = await measurementRepository.GetRecipeMeasurementsByIdAsync(recipeId);

            //Assert
            Assert.Equal(expectedAmount, actual.Count());
        }

        [Fact]
        public async Task GetMeasurementByRecipeId_ShouldReturnEmptyList()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var recipeId = 85;
            var expectedAmount = 0;

            //Actual
            var actual = await measurementRepository.GetRecipeMeasurementsByIdAsync(recipeId);

            //Assert
            Assert.Equal(expectedAmount, actual.Count());
        }

        [Fact]
        public async Task Insert_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurement = new Measurement()
            {
                RecipeId = 1,
                IngredientId = 1,
                Quantity = "any",
                Consistency = "any"
            };
            var expectedCount = 13;

            //Actual
            await measurementRepository.InsertAsync(measurement);
            var actual = await measurementRepository.GetByIdAsync(measurement.Id);
            var actualCount = await dbContext.Measurements.CountAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task Update_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurement = new Measurement()
            {
                Id = 1,
                RecipeId = 2,
                IngredientId = 7,
                Quantity = "Edited",
                Consistency = "Edited"
            };

            //Actual
            await measurementRepository.UpdateAsync(measurement);
            var actual = await measurementRepository.GetByIdAsync(measurement.Id);

            //Assert
            Assert.Equal(measurement.RecipeId, actual.RecipeId);
            Assert.Equal(measurement.IngredientId, actual.IngredientId);
            Assert.Equal(measurement.Quantity, actual.Quantity);
            Assert.Equal(measurement.Consistency, actual.Consistency);
        }

        [Fact]
        public async Task Update_ShouldNotWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurement = new Measurement()
            {
                Id = 105,
                RecipeId = 1,
                IngredientId = 3,
                Quantity = "None",
                Consistency = "None"
            };

            //Actual

            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>
                (() => measurementRepository.UpdateAsync(measurement));
        }

        [Fact]
        public async Task Delete_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurementId = 1;
            var expectedCount = await dbContext.Measurements.CountAsync() - 1;

            //Actual
            await measurementRepository.DeleteAsync(measurementId);
            var actual = await measurementRepository.GetByIdAsync(measurementId);
            var actualCount = await dbContext.Measurements.CountAsync();

            //Assert
            Assert.Null(actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task Delete_ShouldNotWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            measurementRepository = new MeasurementRepository(dbContext);

            //Arrange
            var measurementId = 41;

            //Actual

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>
                (() => measurementRepository.DeleteAsync(measurementId));
        }
    }
}
