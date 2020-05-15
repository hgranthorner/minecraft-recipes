using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Recipes.Data.Models;

namespace Recipes.DataImport
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = "recipe-db.cmvt1ttz84am.us-east-1.rds.amazonaws.com";
            var database = "recipes";
            var username = "postgres";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var cs = $"Host={server};Port=5432;Database={database};User Id={username};Password={password};";
            await using var context = new RecipesContext(cs, false);
            using var http = new HttpClient();
            var stream =
                await http.GetStreamAsync(
                    "https://launcher.mojang.com/v1/objects/e3f78cd16f9eb9a52307ed96ebec64241cc5b32d/client.jar");
            using var zip = new ZipArchive(stream);

            var serializer = new JsonSerializer();

            RemoveCurrentData(context);
            await LoadItemsAsync(context, zip);
            await LoadRecipesAndPatternKeysAsync(zip, serializer, context);
        }

        private static async Task LoadRecipesAndPatternKeysAsync(
            ZipArchive zip,
            JsonSerializer serializer,
            RecipesContext context)
        {
            var objects = zip.Entries
                .Where(e => e.FullName.StartsWith("data/minecraft/recipes/") && e.FullName.EndsWith(".json"))
                .Select(entry =>
                {
                    var stream = zip.GetEntry(entry.FullName)?.Open();
                    using var streamReader =
                        new StreamReader(
                            stream ?? throw new NullReferenceException($"Can't find zipped file {entry.FullName}"));
                    using var jsonReader = new JsonTextReader(streamReader);
                    dynamic obj = serializer.Deserialize(jsonReader);
                    stream.Close();
                    return (obj, entry.Name);
                })
                .Where(o => o.obj.type == "minecraft:crafting_shaped")
                .Select(o =>
                {
                    var (obj, name) = o;
                    string resultItemName = null;
                    if (obj.result != null)
                        resultItemName = obj.result.item.ToString();
                    var resultItem = resultItemName != null
                        ? context.Items.FirstOrDefault(i => i.Name == resultItemName.Replace("minecraft:", ""))
                        : null;
                    var recipe = new Recipe
                    {
                        Group = obj.group,
                        Name = name.Replace(".json", ""),
                        Type = obj.type,
                        Pattern = string.Join('\n', obj.pattern),
                        Result = resultItem,
                        ResultCount = obj.result?.count ?? 0
                    };

                    var keys = new List<PatternKey>();
                    var chars = new HashSet<char>();
                    foreach (var line in obj.pattern)
                    {
                        var s = (string) line?.ToString();
                        if (s == null || !s.Any()) continue;
                        foreach (var c in s)
                        {
                            chars.Add(c);
                        }
                    }

                    foreach (var c in chars)
                    {
                        if (c == '\n') continue;
                        var objectKey = c.ToString();
                        try
                        {
                            if (obj.key == null
                                || obj.key[objectKey] == null) continue;
                            var type = (Type) obj.key[objectKey].GetType();
                            if (type == typeof(JArray) || obj.key[objectKey].item == null) continue;
                            var jobject = obj.key[objectKey].item;
                            var itemName = ((string) jobject).Replace("minecraft:", "");
                            var item = context.Items.FirstOrDefault(i => i.Name == itemName);
                            if (item != null)
                            {
                                keys.Add(new PatternKey
                                {
                                    Character = c.ToString(),
                                    Item = item,
                                    Recipe = recipe
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine(
                                $"Values: \nrecipe name:{name}\nkey: {c}\nvalue: {obj.key[objectKey].item}.");
                            throw;
                        }
                    }

                    return (recipe, keys);
                });

            var keys = new List<PatternKey>();
            var recipes = new List<Recipe>();
            foreach (var o in objects)
            {
                recipes.Add(o.recipe);
                keys.AddRange(o.keys);
            }

            await context.Recipes.AddRangeAsync(recipes);
            await context.PatternKeys.AddRangeAsync(keys);
            var count = await context.SaveChangesAsync();

            Console.WriteLine($"Saved {count} recipes and pattern keys.");
        }

        private static void RemoveCurrentData(RecipesContext context)
        {
            context.Items.RemoveRange(context.Items);
            context.Recipes.RemoveRange(context.Recipes);
            context.PatternKeys.RemoveRange(context.PatternKeys);
        }

        private static async Task LoadItemsAsync(RecipesContext context, ZipArchive zip)
        {
            var items = zip.Entries.Where(e =>
                    e.FullName.StartsWith("assets/minecraft/models/item") && e.FullName.EndsWith(".json"))
                .Select(entry =>
                {
                    return new Item
                    {
                        Name = entry.Name.Replace(".json", string.Empty)
                    };
                });
            context.Items.AddRange(items);

            var count = await context.SaveChangesAsync();

            Console.WriteLine($"Saved {count} items.");
        }
    }
}