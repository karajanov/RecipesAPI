using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }
    }
}
