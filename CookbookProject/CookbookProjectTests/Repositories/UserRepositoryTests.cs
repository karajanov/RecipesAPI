using CookbookProject.Models;
using CookbookProject.Services.Repository;
using CookbookProject.Services.Repository.Interfaces;
using CookbookProjectTests.Internal;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace CookbookProjectTests.Repositories
{
    public class UserRepositoryTests
    {
        private IUserRepository userRepository;

        public UserRepositoryTests()
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectUser()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var userId = 1;

            //Actual
            var actual = await userRepository.GetByIdAsync(userId);

            //Assert
            Assert.Equal(userId, actual.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var userId = 22;

            //Actual
            var actual = await userRepository.GetByIdAsync(userId);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task IsEmailTaken_ShouldReturnTrue()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var email = "karajanovb@yahoo.com";

            //Actual
            var actual = await userRepository.IsEmailTakenAsync(email);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task IsEmailTaken_ShouldReturnFalse()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var email = "example@email.com";

            //Actual
            var actual = await userRepository.IsEmailTakenAsync(email);

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task IsUsernameTaken_ShouldReturnTrue()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var username = "UserOne";

            //Actual
            var actual = await userRepository.IsUsernameTakenAsync(username);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task IsUsernameTaken_ShouldReturnFalse()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var username = "Test";

            //Actual
            var actual = await userRepository.IsUsernameTakenAsync(username);

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task GetIdByUsername_ShouldReturnCorrectResult()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var expectedId = 1;
            var username = "UserOne";

            //Actual
            var actual = await userRepository.GetIdByUsernameAsync(username);

            //Assert
            Assert.Equal(expectedId, actual);

        }

        [Fact]
        public async Task GetIdByUsername_ShouldReturnZero()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var username = "Test";

            //Actual
            var actual = await userRepository.GetIdByUsernameAsync(username);

            //Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task Insert_ShouldWork()
        {
            using var factory = new SQLiteDbContextFactory();
            await using var dbContext = factory.CreateContext();
            userRepository = new UserRepository(dbContext, null);

            //Arrange
            var user = new User()
            {
                Username = "TestUsername",
                Email = "example@email.com",
                Password = new byte[] { 0 }
            };
            var expectedCount = 2;

            //Actual
            await userRepository.InsertAsync(user);
            var actual = await userRepository.GetByIdAsync(user.Id);
            var userList = await dbContext.Users.ToListAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, userList.Count);
            Assert.Equal(user.Username, actual.Username);
            Assert.Equal(user.Email, actual.Email);
            Assert.Equal(user.Password, actual.Password);
        }
    }
}
