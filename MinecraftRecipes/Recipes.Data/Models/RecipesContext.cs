using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recipes.Core.Models;


namespace Recipes.Data.Models
{
    public class RecipesContext : DbContext
    {
        public RecipesContext()
        {
        }

        public RecipesContext(DbContextOptions options) : base(options)
        {
        }

        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public RecipesContext(string connectionString, bool enableSensitiveDataLogging) : this(
            new DbContextOptionsBuilder()
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging(enableSensitiveDataLogging)
                .UseLoggerFactory(MyLoggerFactory).Options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<PatternKey> PatternKeys { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}