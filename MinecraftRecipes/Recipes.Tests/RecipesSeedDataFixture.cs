using System;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Tests
{
    public class RecipesSeedDataFixture : IDisposable
    {
        public TestRecipesContext Context;

        public void Dispose()
        {
            Context.Dispose();
        }

        public void SeedDatabase(string testName)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase($"TestDb_Fixture_{testName}");
            builder.EnableSensitiveDataLogging();
            Context = new TestRecipesContext(builder.Options);
            Context.Database.EnsureCreated();
        }
    }
}