using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Recipes.Data.Models;

namespace Recipes.Tests
{
    public class RecipesSeedDataFixture : IDisposable
    {
        public RecipesContext Context;

        public RecipesSeedDataFixture()
        {
            var builder = new DbContextOptionsBuilder<RecipesContext>();
            builder.UseInMemoryDatabase("TestDb_Fixture");
            Context = new RecipesContext(builder.Options);
            Seed.SeedData(Context);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}