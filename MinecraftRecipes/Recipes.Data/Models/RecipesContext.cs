using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


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

        public static readonly ILoggerFactory MyLoggerFactory
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


    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public string Pattern { get; set; }
        public List<PatternKey> PatternKeys { get; set; } = new List<PatternKey>();
        public Item Result { get; set; }
        public int ResultCount { get; set; }
    }

    public class PatternKey
    {
        public int Id { get; set; }
        public string Character { get; set; }

        public Item Item { get; set; }
        public Recipe Recipe { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}