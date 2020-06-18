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
        private const string DatabaseName = "TestingDb";

        public static RecipesContext Context
        {
            get
            {
                var builder = new DbContextOptionsBuilder<TestRecipesContext>();
                builder.UseInMemoryDatabase(DatabaseName);
                builder.EnableSensitiveDataLogging();
                return new TestRecipesContext(builder.Options);
            }
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

                services.AddDbContext<TestRecipesContext>(options =>
                {
                    options.UseInMemoryDatabase(DatabaseName);
                    options.EnableSensitiveDataLogging();
                });

                services.AddTransient<RecipesContext>(serviceProvider =>
                    serviceProvider.GetRequiredService<TestRecipesContext>());

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TestRecipesContext>();

                // Ensure the database is created.
                db.Database.EnsureCreated();
            });
        }
    }
}