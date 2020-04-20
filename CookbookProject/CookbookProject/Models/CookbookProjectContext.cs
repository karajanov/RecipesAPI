using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CookbookProject.Models
{
    public class CookbookProjectContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Cuisine> Cuisine { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Recipe> Recipes { get; set; }

        public virtual DbSet<Ingredient> Ingredients { get; set; }

        public virtual DbSet<Measurement> Measurements { get; set; }

        public CookbookProjectContext(DbContextOptions<CookbookProjectContext> options,
            IConfiguration config) : base(options)
        {
            Configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                .UseIdentityColumn(1, 1);

                entity.HasIndex(u => new { u.Email, u.Username })
                .IsUnique(true);

                entity.Property(u => u.Email)
                .HasMaxLength(320)
                .IsUnicode(false)
                .IsRequired(true);

                entity.Property(u => u.Username)
                .HasMaxLength(20)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(u => u.Password)
                .HasColumnType("binary(32)")
                .IsRequired(true);
            });

            modelBuilder.Entity<Cuisine>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                .UseIdentityColumn(1, 1);

                entity.HasIndex(c => c.Title)
                .IsUnique(true);

                entity.Property(c => c.Title)
                .HasMaxLength(25)
                .IsUnicode(true)
                .IsRequired(true);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                .UseIdentityColumn(1, 1);

                entity.HasIndex(c => c.Title)
                .IsUnique(true);

                entity.Property(c => c.Title)
                .HasMaxLength(25)
                .IsUnicode(true)
                .IsRequired(true);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                .UseIdentityColumn(1, 1);

                entity.Property(r => r.Title)
                .HasMaxLength(30)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(r => r.PrepTime)
                .HasMaxLength(15)
                .IsUnicode(true)
                .IsRequired(false);

                entity.Property(r => r.Instructions)
                .HasMaxLength(800)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(r => r.ImagePath)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired(false);

                entity.Property(r => r.CuisineId)
                .IsRequired(false);

                entity.Property(r => r.CategoryId)
                .IsRequired(false);

                entity.Property(r => r.UserId)
                .IsRequired(true);

                entity.HasOne(r => r.Cuisine)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CuisineId)
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(r => r.Category)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Id)
                .UseIdentityColumn(1, 1);

                entity.HasIndex(i => i.Title)
                .IsUnique(true);

                entity.Property(i => i.Title)
                .HasMaxLength(30)
                .IsUnicode(true)
                .IsRequired(true);
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Id)
                .UseIdentityColumn(1, 1);

                entity.Property(m => m.RecipeId)
                .IsRequired(false);

                entity.Property(m => m.IngredientId)
                .IsRequired(false);

                entity.Property(m => m.Quantity)
                .HasMaxLength(20)
                .IsUnicode(true)
                .IsRequired(false);

                entity.Property(m => m.Consistency)
                .HasMaxLength(20)
                .IsUnicode(true)
                .IsRequired(false);

                entity.HasOne(m => m.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(m => m.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Ingredient)
                .WithMany(i => i.Recipes)
                .HasForeignKey(m => m.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "karajanovb@yahoo.com",
                    Username = "UserOne",
                    Password = new byte[]
                    { // bytes of hashed password
                      49, 162, 213, 40, 38, 131, 237, 179,
                      162, 44, 86, 95, 25, 154, 169, 111,
                      185, 255, 179, 16, 122, 243, 90, 173, 
                      146, 238, 28, 213, 103, 207, 194, 93
                    }
                });

            modelBuilder.Entity<Cuisine>().HasData(
                new Cuisine { Id = 1, Title = "Mexican" },
                new Cuisine { Id = 2, Title = "Italian" },
                new Cuisine { Id = 3, Title = "American" },
                new Cuisine { Id = 4, Title = "British" },
                new Cuisine { Id = 5, Title = "Indian" });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Pasta" },
                new Category { Id = 2, Title = "Fast Food" },
                new Category { Id = 3, Title = "Dessert" },
                new Category { Id = 4, Title = "Pastry" },
                new Category { Id = 5, Title = "Stew" });

            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Title = "All-purpose Flour" },
                new Ingredient { Id = 2, Title = "Cherries" },
                new Ingredient { Id = 3, Title = "Eggs" },
                new Ingredient { Id = 4, Title = "Sugar" },
                new Ingredient { Id = 5, Title = "Milk" },
                new Ingredient { Id = 6, Title = "Spaghetti" },
                new Ingredient { Id = 7, Title = "Garlic" },
                new Ingredient { Id = 8, Title = "Tomatoes" },
                new Ingredient { Id = 9, Title = "Beef" },
                new Ingredient { Id = 10, Title = "Parmesan" },
                new Ingredient { Id = 11, Title = "Tortillas" },
                new Ingredient { Id = 12, Title = "Salmon" });

            modelBuilder.Entity<Recipe>().HasData(
                new Recipe
                {
                    Id = 1,
                    Title = "Fish Tacos",
                    PrepTime = "25 min",
                    ImagePath = null,
                    UserId = 1,
                    CuisineId = 1,
                    CategoryId = 2,
                    Instructions = "Instructions for Fish Tacos"
                },
                new Recipe
                {
                    Id = 2,
                    Title = "Cherry Cobbler",
                    PrepTime = "45 min",
                    UserId = 1,
                    ImagePath = null,
                    CuisineId = 3,
                    CategoryId = 3,
                    Instructions = "Instructions for Cherry Cobbler"
                },
                new Recipe
                {
                    Id = 3,
                    Title = "Beef Ragu",
                    PrepTime = "55 min",
                    UserId = 1,
                    ImagePath = null,
                    CuisineId = 2,
                    CategoryId = 1,
                    Instructions = "Instructions for Beef Ragu"
                });

            modelBuilder.Entity<Measurement>().HasData(
                new Measurement
                {
                    Id = 1,
                    RecipeId = 1,
                    IngredientId = 11,
                    Quantity = "3",
                    Consistency = null
                },
                new Measurement
                {
                    Id = 2,
                    RecipeId = 1,
                    IngredientId = 12,
                    Quantity = "200 gr",
                    Consistency = "fillet"
                },
                new Measurement
                {
                    Id = 3,
                    RecipeId = 2,
                    IngredientId = 1,
                    Quantity = "250 gr",
                    Consistency = null
                },
                new Measurement
                {
                    Id = 4,
                    RecipeId = 2,
                    IngredientId = 2,
                    Quantity = "100 gr",
                    Consistency = "frozen"
                },
                new Measurement
                {
                    Id = 5,
                    RecipeId = 2,
                    IngredientId = 3,
                    Quantity = "4",
                    Consistency = "beaten"
                },
                new Measurement
                {
                    Id = 6,
                    RecipeId = 2,
                    IngredientId = 4,
                    Quantity = "150 gr",
                    Consistency = null
                },
                new Measurement
                {
                    Id = 7,
                    RecipeId = 2,
                    IngredientId = 5,
                    Quantity = "300 ml",
                    Consistency = "Skimmed"
                },
                new Measurement
                {
                    Id = 8,
                    RecipeId = 3,
                    IngredientId = 6,
                    Quantity = "100 gr",
                    Consistency = null
                },
                new Measurement
                {
                    Id = 9,
                    RecipeId = 3,
                    IngredientId = 7,
                    Quantity = "1 clove",
                    Consistency = "crushed"
                },
                new Measurement
                {
                    Id = 10,
                    RecipeId = 3,
                    IngredientId = 8,
                    Quantity = "1 can",
                    Consistency = null
                },
                new Measurement
                {
                    Id = 11,
                    RecipeId = 3,
                    IngredientId = 9,
                    Quantity = "200 gr",
                    Consistency = "minced"
                },
                new Measurement
                {
                    Id = 12,
                    RecipeId = 3,
                    IngredientId = 10,
                    Quantity = "2 teaspoons",
                    Consistency = null
                });
        }
    }
}
