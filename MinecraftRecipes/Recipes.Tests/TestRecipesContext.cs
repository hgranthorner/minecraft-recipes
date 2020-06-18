using Microsoft.EntityFrameworkCore;
using Recipes.Core.Models;
using Recipes.Data.Models;

namespace Recipes.Tests
{
    public class TestRecipesContext : RecipesContext
    {
        public TestRecipesContext()
        {
        }

        public TestRecipesContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    Name = "Item1"
                },
                new Item
                {
                    Id = 2,
                    Name = "Item2"
                },
                new Item
                {
                    Id = 3,
                    Name = "Item3"
                },
                new Item
                {
                    Id = 4,
                    Name = "Item4"
                },
                new Item
                {
                    Id = 5,
                    Name = "Item5"
                }
            );
            modelBuilder.Entity<Recipe>().HasData(
                new Recipe
                {
                    Id = 1,
                    Group = "",
                    Name = "Recipe1",
                    Pattern = "",
                    ResultId = 1,
                    PatternKeys = null,
                    Type = "type",
                    ResultCount = 1
                },
                new Recipe
                {
                    Id = 2,
                    Group = "",
                    Name = "Recipe2",
                    Pattern = "",
                    ResultId = 2,
                    PatternKeys = null,
                    Type = "type",
                    ResultCount = 2
                },
                new Recipe
                {
                    Id = 3,
                    Group = "",
                    Name = "Recipe3",
                    Pattern = "",
                    ResultId = 3,
                    PatternKeys = null,
                    Type = "type",
                    ResultCount = 3
                },
                new Recipe
                {
                    Id = 4,
                    Group = "",
                    Name = "Recipe4",
                    Pattern = "",
                    ResultId = 4,
                    PatternKeys = null,
                    Type = "type",
                    ResultCount = 4
                }
            );
            modelBuilder.Entity<PatternKey>().HasData(
                new PatternKey
                {
                    Character = "",
                    Id = 1,
                    ItemId = 1,
                    RecipeId = 1
                },
                new PatternKey
                {
                    Character = "",
                    Id = 2,
                    ItemId = 2,
                    RecipeId = 1
                },
                new PatternKey
                {
                    Character = "",
                    Id = 3,
                    ItemId = 3,
                    RecipeId = 2
                },
                new PatternKey
                {
                    Character = "",
                    Id = 4,
                    ItemId = 4,
                    RecipeId = 2
                },
                new PatternKey
                {
                    Character = "",
                    Id = 5,
                    ItemId = 1,
                    RecipeId = 3
                },
                new PatternKey
                {
                    Character = "",
                    Id = 6,
                    ItemId = 2,
                    RecipeId = 3
                }
            );
        }
    }
}