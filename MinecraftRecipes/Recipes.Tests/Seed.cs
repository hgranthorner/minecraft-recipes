using System.Collections.Generic;
using Recipes.Core.Models;
using Recipes.Data.Models;

namespace Recipes.Tests
{
    public static class Seed
    {
        public static readonly IList<Item> Items = new List<Item>
        {
            new Item
            {
                Id = 0,
                Name = "Item1"
            },
            new Item
            {
                Id = 0,
                Name = "Item2"
            },
            new Item
            {
                Id = 0,
                Name = "Item3"
            },
            new Item
            {
                Id = 0,
                Name = "Item4"
            },
            new Item
            {
                Id = 0,
                Name = "Item5"
            }
        };

        public static readonly IList<Recipe> Recipes = new List<Recipe>
        {
            new Recipe
            {
                Id = 0,
                Group = "",
                Name = "Recipe1",
                Pattern = "",
                Result = Items[1],
                PatternKeys = null,
                Type = "type",
                ResultCount = 1
            },
            new Recipe
            {
                Id = 0,
                Group = "",
                Name = "Recipe2",
                Pattern = "",
                Result = Items[2],
                PatternKeys = null,
                Type = "type",
                ResultCount = 2
            },
            new Recipe
            {
                Id = 0,
                Group = "",
                Name = "Recipe3",
                Pattern = "",
                Result = Items[3],
                PatternKeys = null,
                Type = "type",
                ResultCount = 3
            },
            new Recipe
            {
                Id = 0,
                Group = "",
                Name = "Recipe4",
                Pattern = "",
                Result = Items[4],
                PatternKeys = null,
                Type = "type",
                ResultCount = 4
            }
        };

        public static readonly IList<PatternKey> PatternKeys = new List<PatternKey>
        {
            new PatternKey
            {
                Character = "",
                Id = 0,
                Item = Items[0],
                Recipe = Recipes[0]
            },
            new PatternKey
            {
                Character = "",
                Id = 0,
                Item = Items[1],
                Recipe = Recipes[0]
            },
            new PatternKey
            {
                Character = "",
                Id = 0,
                Item = Items[2],
                Recipe = Recipes[1]
            },
            new PatternKey
            {
                Character = "",
                Id = 0,
                Item = Items[3],
                Recipe = Recipes[1]
            },
            new PatternKey
            {
                Character = "",
                Id = 0,
                Item = Items[0],
                Recipe = Recipes[2]
            },
            new PatternKey
            {
                Character = "",
                Id = 0,
                Item = Items[1],
                Recipe = Recipes[2]
            }
        };

        public static void SeedData(RecipesContext context)
        {
            var items = Items;
            var recipes = Recipes;
            var keys = PatternKeys;
            context.Items.AddRange(items);
            context.Recipes.AddRange(recipes);
            context.PatternKeys.AddRange(keys);
            context.SaveChanges();
        }
    }
}