using CookbookProject.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace CookbookProjectTests.Internal
{
    public class SQLiteDbContextFactory : IDisposable
    {
        private DbConnection connection;

        private DbContextOptions<CookbookProjectContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<CookbookProjectContext>()
                .UseSqlite(connection)
                .Options;
        }

        public CookbookProjectContext CreateContext()
        {
            if (connection == null)
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var options = CreateOptions();
                using var context = new CookbookProjectContext(options);
                context.Database.EnsureCreated();
            }

            return new CookbookProjectContext(CreateOptions());
        }

        public void Dispose()
        {
            if (connection == null)
            {
                connection.Dispose();
                connection = null;
            }
        }
    }
}
