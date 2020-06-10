using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recipes.API;
using Recipes.Data.Models;

namespace Recipes.Tests
{
    public class RecipesWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private static string DatabaseName = "TestingDb";

        public static RecipesContext CreateDatabase()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(DatabaseName);
            return new RecipesContext(builder.Options);
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<RecipesContext>));

                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<RecipesContext>(options =>
                {
                    options.UseInMemoryDatabase(DatabaseName);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<RecipesContext>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    Seed.SeedData(db);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred seeding the " +
                                        $"database with test messages. Error: {ex.Message}");
                }
            });
        }
    }
}