using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Recipes.Data.Models;

namespace Recipes.DataImport
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var server = "recipe-db.cmvt1ttz84am.us-east-1.rds.amazonaws.com";
            var database = "recipes";
            var username = "postgres";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var cs = $"Host={server};Port=5432;Database={database};User Id={username};Password={password};";
            using var context = new RecipesContext(cs);

            context.Recipes.RemoveRange(context.Recipes);

            using var http = new HttpClient();
            var stream = await http.GetStreamAsync("https://launcher.mojang.com/v1/objects/e3f78cd16f9eb9a52307ed96ebec64241cc5b32d/client.jar");
            using var zip = new ZipArchive(stream);

            var serializer = new JsonSerializer();

            context.Items.RemoveRange(context.Items);
            var items = zip.Entries.Where(e => e.FullName.StartsWith("assets/minecraft/models/item") && e.FullName.EndsWith(".json"))
                .Select(entry =>
                {
                    return new Item
                    {
                        Name = entry.Name.Replace(".json", string.Empty)
                    };
                });
            context.Items.AddRange(items);

            var count = await context.SaveChangesAsync();

            Console.WriteLine($"Saved {count} items");
            //
            // var objects = zip.Entries.Where(e => e.FullName.StartsWith("data/minecraft/recipes/") && e.FullName.EndsWith(".json"))
            //     .Select(entry =>
            //     {
            //         var stream = zip.GetEntry(entry.FullName).Open();
            //         using var streamReader = new StreamReader(stream);
            //         using var jsonReader = new JsonTextReader(streamReader);
            //         dynamic obj = serializer.Deserialize(jsonReader);
            //         stream.Close();
            //
            //
            //         return obj;
            //     })
            //     .Where(o => o.type == "minecraft:crafting_shaped");
            //
            // foreach (var o in objects)
            // {
            //     var chars = new HashSet<char>();
            //     foreach (var line in o.pattern)
            //     {
            //         foreach (var c in line)
            //         {
            //             chars.Add(c);
            //         }
            //     }
            //     chars.Select(c =>
            //     {
            //         var patternKey = new PatternKey();
            //         patternKey.Character = c.ToString();
            //         patternKey.Item
            //     })
            //
            //     var recipe = new Recipe
            //     {
            //         Group = o.group,
            //         Type = o.type,
            //         Pattern = string.Join('\n', o.pattern),
            //         Result = o.result?.item,
            //         ResultCount = o.result?.count,
            //         PatternKeys =
            //     };
            // }


            // Console.WriteLine(objects.First());
        }
    }
}
